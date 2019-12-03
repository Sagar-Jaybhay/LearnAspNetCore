using System;
using System.Collections.Generic;
using System.Linq;
using LearnAspCore.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LearnAspCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;

namespace LearnAspCore.Controllers
{
    [Authorize(Roles = "Admin")]
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
                  //  model.Users=new List<string>();
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

        [HttpGet]
        public async Task<IActionResult> EditUsersInRoles(string RoleId)
        {

            ViewBag.RoleId = RoleId;

            var role = await rolesManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.Message = $"Role of {RoleId} of this Id is Not found";
                return View("NotFound");
            }
            else
            {
                var model = new List<UserRoleViewModel>();

                foreach (var users in _userManager.Users)
                {
                    var Users = new UserRoleViewModel()
                    {
                        UserName = users.UserName,
                        UserId = users.Id
                    };

                    if (await _userManager.IsInRoleAsync(users, role.Name))
                    {
                        Users.IsSelected = true;
                    }
                    else
                    {
                        Users.IsSelected = false;
                    }
                    model.Add(Users);
                }

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRoles(List<UserRoleViewModel> model, string RoleId)
        {
            var role = await rolesManager.FindByIdAsync(RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={RoleId} not found";
                return View("NotFound");
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user=await _userManager.FindByIdAsync(model[i].UserId);

                    IdentityResult result = null;

                    if (model[i].IsSelected == true&&!(await _userManager.IsInRoleAsync(user,role.Name)))
                        result= await _userManager.AddToRoleAsync(user, role.Name);
                    else if(!model[i].IsSelected&&await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if(i<(model.Count-1))
                            continue;
                        else
                        {
                            return RedirectToAction("EditRole", new {Id = RoleId});
                        }
                    }
                }

                
            }

            return View("NotFound");

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListOfUsers()
        {
            var users = _userManager.Users;
            return View(users);

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID :{Id} of this customer is not found.";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model=new EditUserViewModel()
            {
                Id = Id,
                City = user.City,
                UserName = user.UserName,
                Email = user.Email,
                Claims = userClaims.Select(c=>c.Value).ToList(),
                Roles = userRoles.ToList()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel userView)
        {
            var user = await _userManager.FindByIdAsync(userView.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID :{userView.Id} of this customer is not found.";
                return View("NotFound");
            }
            else
            {
                user.Email = userView.Email;
                user.City = userView.City;
                user.Email = userView.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(userView);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserID)
        {

            var user =await _userManager.FindByIdAsync(UserID);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("ListOfRoles");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            else
            {
                return View("NotFound");
            }

            return View("NotFound");
        }

    }
}