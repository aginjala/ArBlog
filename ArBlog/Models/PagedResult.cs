namespace ArBlog.Models
{
    public record PagedResult<TResult>(TResult[] Items, int TotalCount);
}
