using System;
using LearnAspCore.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LearnAspCore.Models;

namespace LearnAspCore.Controllers
{
    public class RolemanagController : Controller
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        public RoleManager<IdentityRole> rolesManager { get; set; }
        public RolemanagController(RoleManager<IdentityRole> rolesManager,UserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
            this.rolesManager = rolesManager;
        }

        [HttpGet]
        public IActionResult CreateRoles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoles(RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role=new IdentityRole()
                {
                    Name = roleView.RoleName
                };

               IdentityResult result=await this.rolesManager.CreateAsync(role);

               if (result.Succeeded)
                   return RedirectToAction("ListOfRoles", "Rolemanag");

               foreach (var identityErrorLE in result.Errors)
               {
                   ModelState.AddModelError("",identityErrorLE.Description);
               }
            }

            return View(roleView);
        }

        public IActionResult ListOfRoles()
        {
            var list = this.rolesManager.Roles;
            return View(list);

        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role =await this.rolesManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessages = $"Role of given id {id} is not found.";
                return View("NotFound");
            }
            else
            {
                var model=new EditRoleViewModel()
                {
                    RoleName = role.Name,
                    Id =(role.Id),
                    
                };

                foreach (var users in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(users, role.Name))
                    {
                        model.Users.Add(users.UserName);
                    }
                    
                }

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await this.rolesManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessages = $"Role of given id {model.Id} is not found.";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var res=await this.rolesManager.UpdateAsync(role);
                if (res.Succeeded)
                {
                    return RedirectToAction("ListOfRoles", "Rolemanag");
                }

                foreach (var erros in res.Errors)
                {
                    ModelState.AddModelError("",erros.Description);
                }
            }
            return View(model);
        }


    }
}