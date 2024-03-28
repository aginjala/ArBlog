using ArBlog.Data;
using ArBlog.Data.Entities;
using ArBlog.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArBlog.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<Category[]> GetCategoriesAsync()
        {
            var categories = await ExecuteOnContextAsync(async context =>
            {
                return await context.Categories.AsNoTracking().ToArrayAsync();
            });
            return categories;
        }

        public async Task<Category> SaveCategoryAsync(Category category)
        {
            var savedCategory = await ExecuteOnContextAsync(async context =>
            {
                if (category.Id == 0)
                {
                    if (await context.Categories.AsNoTracking().AnyAsync(c => c.Name == category.Name))
                    {
                        throw new InvalidOperationException($"Category with name '{category.Name}' already exist.");
                    }
                    category.Slug = category.Name.ToSlug();
                    category.CreatedAt = category.UpdatedAt = DateTime.Now;
                    await context.Categories.AddAsync(category);

                }
                else
                {
                    if (await context.Categories.AsNoTracking().AnyAsync(c =>
                            c.Name.ToLower() == category.Name.ToLower()
                            && c.Id != category.Id))
                    {
                        throw new InvalidOperationException($"Category with name '{category.Name}' already exist.");
                    }
                    var dbCategory = await context.Categories.FindAsync(category.Id) ?? throw new KeyNotFoundException($"Category with id '{category.Id}' is not found");

                    category.Slug = dbCategory.Slug;
                    dbCategory.Name = category.Name;
                    dbCategory.UpdatedAt = DateTime.Now;
                    dbCategory.ShowOnNavbar = category.ShowOnNavbar;
                }

                await context.SaveChangesAsync();
                return category;
            });

            return savedCategory;
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug) =>
            await ExecuteOnContextAsync(async context =>
            {
                return await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);
            });
    }
}
