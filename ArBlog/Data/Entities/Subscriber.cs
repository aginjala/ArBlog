using System.ComponentModel.DataAnnotations;

namespace ArBlog.Data.Entities
{
	public class Subscriber
	{
		public int Id { get; set; }
		[Required, MaxLength(100)]
		public string Email { get; set; }
		[Required, MaxLength(100)]
	    public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime SubscribedOn { get; set; }
	}
}
