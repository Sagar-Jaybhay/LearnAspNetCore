using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnAspCore.Models;
using LearnAspCore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LearnAspCore.Controllers
{
    public class AccountController : Controller
    {
        private ILogger logger;
        private readonly UserManager<ExtendedIdentityUser> _user;
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;

        public AccountController(ILogger<AccountController> logger,UserManager<ExtendedIdentityUser> user,SignInManager<ExtendedIdentityUser> signInManager)
        {
            this.logger = logger;
            _user = user;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsUsedEmailID(string Email)
        {
            //if (!string.IsNullOrEmpty(EmailID))
            {
             var user= await _user.FindByEmailAsync(Email);

             if (user == null)
             {
                 return Json(true);
             }
             else
             {
                 return Json($"Email {Email} this Email ID already in Used.");
             }
            }
            
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationVIewModel model)
        {
            if (ModelState.IsValid)
            {
                var user=new ExtendedIdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City,
                    Zip = model.Zip
                };
             var result= await  _user.CreateAsync(user, model.Password);

             if (result.Succeeded)
             {
                 _signInManager.SignInAsync(user, true);
                 return RedirectToAction("List", "Home");
             }

             foreach (var errors in result.Errors)
             {
                 ModelState.AddModelError("",errors.Description);
             }

            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("List", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                    else
                    {
                        return RedirectToAction("List", "Home");
                    }
                    //return RedirectToAction("List", "Home");
                }

                ModelState.AddModelError("", "Invalid Login");           
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        //
    }
}