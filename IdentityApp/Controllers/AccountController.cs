using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);
                    }

                    else if (result.IsLockedOut)
                    {
                        var logOutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = logOutDate.Value - DateTime.UtcNow;

                        ModelState.AddModelError("", $"Hesabınız kilitlendi, lütfen {timeLeft.Minutes} dakika sonra tekrar deneyin");
                    }
                    else
                    {
                    ModelState.AddModelError("", "hatalı parola");
                        
                    }
                }
                else
                {
                    ModelState.AddModelError("", "hatalı email");
                }
                
            }
            return View(model);
        }
    }
}