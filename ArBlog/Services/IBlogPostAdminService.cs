using ArBlog.Data.Entities;
using ArBlog.Models;

namespace ArBlog.Services
{
    public interface IBlogPostAdminService
    {
        Task<PagedResult<BlogPost>> GetBlogPostsAsync(int startIndex, int pageSize);

        Task<BlogPost?> GetBlogPostAsync(int id);

        Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, string userId);
    }
}