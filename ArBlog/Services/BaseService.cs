using ArBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace ArBlog.Services
{
    public class BaseService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        protected BaseService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected async Task<TResult> ExecuteOnContextAsync<TResult>(Func<ApplicationDbContext, Task<TResult>> query)
        {
            using var context = _contextFactory.CreateDbContext();
            return await query(context);
        }
    }
}
