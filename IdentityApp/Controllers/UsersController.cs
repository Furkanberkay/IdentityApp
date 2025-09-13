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

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userMenager.FindByIdAsync(id);

            if (user != null)
            {
                return View(new EditViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName,
                    Username = user.UserName!,
                });
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var user = await _userMenager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.UserName = model.Username;

                    var result = await _userMenager.UpdateAsync(user);

                    if (result.Succeeded && !string.IsNullOrEmpty(model.PassWord))
                    {
                        await _userMenager.RemovePasswordAsync(user);
                        await _userMenager.AddPasswordAsync(user, model.PassWord);
                    }

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }


                }



            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userMenager.FindByIdAsync(id);

            if (user != null)
            {
                await _userMenager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
} 