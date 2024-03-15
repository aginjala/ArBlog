using ArBlog.Data;
using ArBlog.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArBlog.Services
{
	internal static class AdminAccount
	{
		public const string Role = "Admin";
		public const string Name = "Ashok Reddy";
		public const string Email = "arblog@email.com";
		public const string Password = "Admin@123";
	}

	public class SeedService : ISeedService
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IWebHostEnvironment _env;

		public SeedService(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_env = env;
		}

		private async Task MigrateDatabaseAsync()
		{
			if (_env.IsDevelopment() 
				&& (await _context.Database.GetPendingMigrationsAsync()).Any())
			{
				await _context.Database.MigrateAsync();
			}
		}

		public async Task SeedDataAsync()
		{
			await MigrateDatabaseAsync();

			// Seed Admin Role
			if (await _roleManager.FindByNameAsync(AdminAccount.Role) is null)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole(AdminAccount.Role));
				if (!result.Succeeded)
				{
					throw new Exception($"Failed to create Admin Role. {result?.Errors?.Select(e => e.Description)}");
				}
			}

			// Seed Admin User
			if (await _userManager.FindByEmailAsync(AdminAccount.Email) is null)
			{
				var adminUser = new ApplicationUser
				{
					UserName = AdminAccount.Email,
					Email = AdminAccount.Email,
					EmailConfirmed = true,
					Name = AdminAccount.Name
				};

				var result = await _userManager.CreateAsync(adminUser, AdminAccount.Password);
				if (!result.Succeeded)
				{
					throw new Exception($"Failed to create Admin User. {result?.Errors?.Select(e => e.Description)}");
				}

				result = await _userManager.AddToRoleAsync(adminUser, AdminAccount.Role);
				if (!result.Succeeded)
				{
					throw new Exception($"Failed to add Admin User to Admin Role. {result?.Errors?.Select(e => e.Description)}");
				}
			}

			// Seed Categories
			if (!await _context.Categories.AsNoTracking().AnyAsync())
			{
				await _context.Categories.AddRangeAsync(Category.GetSeedCategories());
				await _context.SaveChangesAsync();
			}
		}
	}
}
