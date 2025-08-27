using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<IdentityUser> _userMenager;
        public UsersController(UserManager<IdentityUser> userManager )
        {
            _userMenager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userMenager.Users);
        }
    }
} 