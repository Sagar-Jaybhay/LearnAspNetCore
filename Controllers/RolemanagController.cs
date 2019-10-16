using LearnAspCore.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnAspCore.Controllers
{
    public class RolemanagController : Controller
    {
        public RoleManager<IdentityRole> rolesManager { get; set; }
        public RolemanagController(RoleManager<IdentityRole> rolesManager)
        {
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


    }
}