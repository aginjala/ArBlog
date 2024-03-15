using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArBlog.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }

}
