using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userMenager;
        public UsersController(UserManager<AppUser> userManager)
        {
            _userMenager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userMenager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModels model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName = model.FullName
                };

                IdentityResult result = await _userMenager.CreateAsync(user, model.PassWord);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
    }
} 