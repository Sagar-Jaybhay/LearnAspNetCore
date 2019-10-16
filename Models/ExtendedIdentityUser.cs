using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LearnAspCore.Models
{
    public class ExtendedIdentityUser: IdentityUser
    {
        public string City { get; set; }
        public string Zip { get; set; }
    }
}
