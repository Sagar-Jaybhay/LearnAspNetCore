using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LearnAspCore.Models;
using Microsoft.AspNetCore.Http;

namespace LearnAspCore.ViewModel
{
    public class StudentCreateViewModel
    {
        
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        public Divi Division { get; set; }
        public IFormFile PhotoPath { get; set; }
    }
}
