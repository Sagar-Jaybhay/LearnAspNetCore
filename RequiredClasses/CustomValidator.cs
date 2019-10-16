using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnAspCore.RequiredClasses
{
    public class CustomValidator:ValidationAttribute
    {
        private string allowedDomain { get; set; }
        public CustomValidator(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string[] array = value.ToString().Split("@");
            if (array.Length > 1)
                return array[1].ToLower() == allowedDomain.ToLower();
            else
            {
                return false;
            }
        }
    }
}
