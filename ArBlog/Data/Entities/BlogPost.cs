using System.ComponentModel.DataAnnotations;

namespace ArBlog.Data.Entities
{
	public class BlogPost
	{
		public int Id { get; set; }
		[Required, MaxLength(100)]
		public string Title { get; set; }
		[MaxLength(100)]
		public string Slug { get; set; }
		[MaxLength(100)]
		public string Image { get; set; }
		[Required, MaxLength(500)]
		public string Introduction { get; set; }
		//[Required]
		public string Content { get; set; }
		[Range(1, short.MaxValue, ErrorMessage = "Please select a Category")]
		public short CategoryId { get; set; }
        public string UserId { get; set; }
		public bool IsPublished { get; set; }
		public int ViewCount { get; set; }
		public bool IsFeatured { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? PublishedAt { get; set; }
		public virtual Category Category { get; set; }
		public virtual ApplicationUser User { get; set; }
	}
}
