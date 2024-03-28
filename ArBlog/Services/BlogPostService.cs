using ArBlog.Data;
using ArBlog.Data.Entities;
using ArBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace ArBlog.Services
{
    public class BlogPostService : BaseService, IBlogPostService
    {
        public BlogPostService(IDbContextFactory<ApplicationDbContext> contextFactory) 
            :base(contextFactory)
        {
        }

        public async Task<BlogPost[]> GetFeaturedBlogPostsAsync(int count, int categoryId = 0)
        {
            return await ExecuteOnContextAsync(async context =>
            {
                var query = context.BlogPosts.AsNoTracking()
                    .Include(b => b.Category)
                    .Include(b => b.User)
                    .Where(b => b.IsPublished && b.IsFeatured);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                var records = await query.OrderByDescending(_ => Guid.NewGuid())
                    .Take(count)
                    .ToArrayAsync();

                if (records.Length < count)
                {
					var additionalRecords = await query.OrderBy(_ => Guid.NewGuid())
                        .Where(b => !b.IsFeatured)
						.Take(count - records.Length)
						.ToArrayAsync();

					records.Concat(additionalRecords).ToArray();
				}

                return records;
            });
        }

        public async Task<BlogPost[]> GetPopularBlogPostsAsync(int count, int categoryId = 0)
        {
            return await ExecuteOnContextAsync(async context =>
            {
                var query = context.BlogPosts.AsNoTracking()
                    .Include(b => b.Category)
                    .Include(b => b.User)
                    .Where(b => b.IsPublished);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                return await query.OrderByDescending(b => b.ViewCount)
                    .Take(count)
                    .ToArrayAsync();
            });
        }

        public async Task<BlogPost[]> GetRecentBlogPostsAsync(int count, int categoryId = 0)
        {
            return await ExecuteOnContextAsync(async context =>
            {
                var query = context.BlogPosts.AsNoTracking()
                    .Include(b => b.Category)
                    .Include(b => b.User)
                    .Where(b => b.IsPublished);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                return await query.OrderByDescending(b => b.PublishedAt)
                    .Take(count)
                    .ToArrayAsync();
            });
        }

        public async Task<DetailPageModel> GetBlogPostBySlugAsync(string slug)
        {
            return await ExecuteOnContextAsync(async context =>
            {
                var blogPost = await ExecuteOnContextAsync(async context =>
                           await context.BlogPosts.AsNoTracking()
                                                  .Include(b => b.Category)
                                                  .Include(b => b.User)
                                                  .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished));
                if(blogPost is null)
                {
                    return DetailPageModel.Empty;
                }

                var relatedBlogPosts = await GetRecentBlogPostsAsync(4, blogPost.CategoryId);
                
                return new DetailPageModel(blogPost ?? new(), relatedBlogPosts);
            });
        }
    }
}
