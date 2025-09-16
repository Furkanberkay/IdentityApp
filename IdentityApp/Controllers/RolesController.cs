using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var role = new AppRole
                {
                    Name = model.Name
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
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

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            if (role.Name == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
            return View(role);

        }

        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                role.Name = model.Name;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            if (role.Name == null)
            {
                return View(role);
            }

            // role null olmadığı garanti → güvenli
            ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
            return View(role);
        }
    }
}