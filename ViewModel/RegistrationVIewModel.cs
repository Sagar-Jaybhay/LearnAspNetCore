using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LearnAspCore.Controllers;
using LearnAspCore.RequiredClasses;
using Microsoft.AspNetCore.Mvc;

namespace LearnAspCore.ViewModel
{
    public class RegistrationVIewModel
    {

        [Required]
        [EmailAddress]
        [Remote(controller:"Account",action: "IsUsedEmailID")]
        [CustomValidator(allowedDomain:"gmail.com",ErrorMessage = "Email Domain Must Be gmail.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Password and Confirm Password not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
        public string Zip { get; set; }
    }
}
