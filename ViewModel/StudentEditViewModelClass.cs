using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnAspCore.ViewModel
{
    public class StudentEditViewModelClass:StudentCreateViewModel
    {
        public  int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
