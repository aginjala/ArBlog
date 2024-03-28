using System.ComponentModel.DataAnnotations;

namespace ArBlog.Data.Entities
{
	public class Category
	{
		public short Id { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(100)]
		public string Slug { get; set; }
		public bool ShowOnNavbar { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<BlogPost> Posts { get; set; }

		public Category Clone() => (Category)MemberwiseClone();

		internal static List<Category> GetSeedCategories()
		{
			return new List<Category>()
			{
				new() { Name = "C#", Slug = "c-sharp", ShowOnNavbar = true },
				new() { Name = "ASP .NET Core", Slug = "asp-net-core", ShowOnNavbar = true },
				new() { Name = "Blazor", Slug = "blazor", ShowOnNavbar = false },
				new() { Name = "HTML", Slug = "html", ShowOnNavbar = true },
				new() { Name = "JavaScript", Slug = "javascript", ShowOnNavbar = true },
				new() { Name = "React", Slug = "react", ShowOnNavbar = true },
				new() { Name = "NextJs", Slug = "nextjs", ShowOnNavbar = false },
				new() { Name = "TailwindCSS", Slug = "tailwind-css", ShowOnNavbar = false },
				new() { Name = "TypeScript", Slug = "typescript", ShowOnNavbar = false },
			};
		}
	}
}
