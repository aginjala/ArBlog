using System.Security.Claims;

namespace ArBlog.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(AppConstants.ClaimNames.FullName) ?? string.Empty;
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
