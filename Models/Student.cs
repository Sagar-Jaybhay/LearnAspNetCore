using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;

namespace LearnAspCore.Models
{
    public class Student
    {
        [RegularExpression(@"^[0-9]*$",ErrorMessage = "Please Enter Number only")]
        public int StudentId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        public Divi Division { get; set; }

        public  string PhotoPath { get; set; }

       
    }
}

