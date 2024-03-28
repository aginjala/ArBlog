using ArBlog.Data.Entities;

namespace ArBlog.Models
{
    public record DetailPageModel(BlogPost BlogPost, BlogPost[] RelatedPosts)
    {
        public static DetailPageModel Empty => new(default, []);

        public bool IsEmpty => BlogPost is null;
    }
}
