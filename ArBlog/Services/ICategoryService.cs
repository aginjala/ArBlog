using ArBlog.Data.Entities;

namespace ArBlog.Services
{
    public interface ICategoryService
    {
        Task<Category[]> GetCategoriesAsync();

        Task<Category?> GetCategoryBySlug(string slug);

        Task<Category> SaveCategoryAsync(Category category);
    }
}