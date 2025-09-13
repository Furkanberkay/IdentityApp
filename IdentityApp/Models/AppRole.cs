using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }

}