using ArBlog.Data;
using ArBlog.Data.Entities;
using ArBlog.Extensions;
using ArBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace ArBlog.Services
{
    public class BlogPostAdminService : BaseService, IBlogPostAdminService
    {
        public BlogPostAdminService(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<BlogPost?> GetBlogPostAsync(int id) => 
            await ExecuteOnContextAsync(async context => 
                await context.BlogPosts.AsNoTracking()
                        .Include(b => b.Category)
                        .FirstOrDefaultAsync(b => b.Id == id));
        

        public async Task<PagedResult<BlogPost>> GetBlogPostsAsync(int startIndex, int pageSize) => 
            await ExecuteOnContextAsync(async context =>
            {
                var totalItems = await context.BlogPosts.AsNoTracking().CountAsync();
                var blogPosts = await context.BlogPosts.AsNoTracking()
                    .Include(b => b.Category)
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToArrayAsync();
                return new PagedResult<BlogPost>(blogPosts, totalItems);
            });

        public async Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, string userId) => 
            await ExecuteOnContextAsync(async context =>
            {
                if (blogPost.Id == 0)
                {
                    var existingTitle = await context.BlogPosts.AsNoTracking().AnyAsync(b => b.Title == blogPost.Title);
                    if (existingTitle)
                    {
                        throw new InvalidOperationException($"Blog post with title '{blogPost.Title}' already exist.");
                    }

                    blogPost.Slug = await GenerateSlugAsync(blogPost);
                    blogPost.CreatedAt = DateTime.UtcNow;
                    blogPost.UserId = userId;

                    if (blogPost.IsPublished)
                        blogPost.PublishedAt = DateTime.UtcNow;

                    await context.BlogPosts.AddAsync(blogPost);
                }
                else
                {
                    var dbBlogPost = await context.BlogPosts.FindAsync(blogPost.Id) ?? throw new KeyNotFoundException($"Blog post with id '{blogPost.Id}' is not found");

                    var existingTitle = await context.BlogPosts.AsNoTracking()
                                                .AnyAsync(b => b.Title == blogPost.Title && b.Id != blogPost.Id);
                    if (existingTitle)
                    {
                        throw new InvalidOperationException($"Blog post with title '{blogPost.Title}' already exist.");
                    }

                    blogPost.Slug = dbBlogPost.Slug;
                    dbBlogPost.Title = blogPost.Title;
                    //dbBlogPost.Slug = blogPost.Title.ToSlug();
                    dbBlogPost.Image = blogPost.Image;
                    dbBlogPost.Introduction = blogPost.Introduction;
                    dbBlogPost.Content = blogPost.Content;
                    dbBlogPost.CategoryId = blogPost.CategoryId;
                    dbBlogPost.IsPublished = blogPost.IsPublished;
                    dbBlogPost.IsFeatured = blogPost.IsFeatured;

                    if(blogPost.IsPublished && !dbBlogPost.IsPublished)
                        dbBlogPost.PublishedAt = DateTime.UtcNow;
                    else if(!blogPost.IsPublished && dbBlogPost.IsPublished)
                        dbBlogPost.PublishedAt = null;
                    else if(blogPost.IsPublished && dbBlogPost.IsPublished && blogPost.PublishedAt != dbBlogPost.PublishedAt)
                        dbBlogPost.PublishedAt = blogPost.PublishedAt;
                }

                await context.SaveChangesAsync();
                return blogPost;
            });

        private async Task<string> GenerateSlugAsync(BlogPost blogPost)
        {
            var originalSlug = blogPost.Title.ToSlug();

            return await ExecuteOnContextAsync(async context =>
            {
                var slug = originalSlug;
                var existingSlug = await context.BlogPosts.AsNoTracking().AnyAsync(b => b.Slug == slug);
                if (existingSlug)
                {
                    var count = 1;
                    while(await context.BlogPosts.AsNoTracking().AnyAsync(b => b.Slug == slug))
                    {
                        slug = originalSlug + $"-{count++}";
                    }
                }
                return slug;
            });
        }
    }
}
