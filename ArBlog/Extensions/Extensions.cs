using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ArBlog.Extensions
{
	public static partial class Extensions
    {
        [GeneratedRegex(@"[^0-9a-z]")]
        private static partial Regex SlugRegex();

        public static string GetUserName(this ClaimsPrincipal principal) => 
            principal.FindFirstValue(AppConstants.ClaimNames.FullName) ?? string.Empty;

        public static string GetUserId(this ClaimsPrincipal principal) => 
            principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public static string ToSlug(this string text)
        {
            return SlugRegex().Replace(text.ToLowerInvariant(), "-")
                              .Replace("--", "-")
                              .Trim('-');
        }

        public static string ToDisplay(this DateTime? dateTime) => dateTime?.ToString("MMM dd") ?? string.Empty;
    }
}
