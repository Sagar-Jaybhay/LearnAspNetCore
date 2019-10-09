using System;
using System.IO;
using LearnAspCore.Models;
using LearnAspCore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnAspCore.Controllers
{
    /// [Route("[controller]")]
    
    public class HomeController : Controller
    {
        private IStudentRepository _repository;
        private IHostingEnvironment histingEnviroment;
        public ILogger loggerObject { get; set; }
        public HomeController(IStudentRepository repository,IHostingEnvironment ihostEnvironment,ILogger<HomeController> logger)
        {
            this._repository = repository;
            this.histingEnviroment = ihostEnvironment;
            loggerObject = logger;
        }

        //[Route("")]
        //[Route("~/")]
        //[Route("[action]")]
        public ViewResult Index()
        {

            loggerObject.LogCritical("LogCritical");
            loggerObject.LogDebug("LogDebug");
            loggerObject.LogError("LogError");
            loggerObject.LogInformation("LogInformation");
            loggerObject.LogTrace("LogTrace");
            loggerObject.LogWarning("LogWarning");

            var v = _repository.GetAllStudent();
            return View(v);
        }

       // [Route("[action]")]
       [AllowAnonymous]
        public ViewResult List()
        {
            var v = _repository.GetAllStudent();
            return View(v);
        }
        //  [Route("[action]/{id?}")]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            //throw new Exception("Error in Methods");
            HmDetailsVM hmDetailsVM = new HmDetailsVM();
            hmDetailsVM.student = _repository.GetStudents(id);

            //if (hmDetailsVM.student == null)
            //{
            //  //  return View("ErrorPage",id);
            //}

            hmDetailsVM.DivisonOfStudent = "9-A";
            ViewBag.TitleNew = "Student Info";
            return View(hmDetailsVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
       
        [ActionName("CreateStudent")]
        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filename =null ;
                
                if (model.PhotoPath != null)
                {
                    string uploads=
                    Path.Combine(
                        histingEnviroment.WebRootPath,"images"
                    );

                    filename= Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName.ToString();
                    string filePath = Path.Combine(uploads, filename);
                    model.PhotoPath.CopyTo(new FileStream(filePath,FileMode.Create));
                }
                Student stud=new Student()
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    Division = model.Division,
                    PhotoPath = filename
                };
                var st = _repository.AddStudent(stud);
                // return RedirectToAction("Details", new {id = st.StudentId});
                return RedirectToAction("Details",new {id= stud.StudentId});
            }
            return View("Create");
        }


       // [ActionName("Edit")]
      //  [HttpPost]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            try
            {
                Student st = _repository.GetStudents(id);
                StudentEditViewModelClass editViewModelClass=new StudentEditViewModelClass()
                {
                    ExistingPhotoPath = st.PhotoPath,
                    Address = st.Address,
                    Division = st.Division,
                     FullName = st.FullName,
                     Id = id
                };
                return View(editViewModelClass);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [ActionName("Edit")]
        [HttpPost]
        public IActionResult Edit(StudentEditViewModelClass model)
        {
            if (ModelState.IsValid)
            {
                string filename = null;

                Student student = _repository.GetStudents(model.Id);

                filename = getFinleName(model, filename);
             

              student.FullName = model.FullName;
              student.Address = model.Address;
              student.Division = model.Division;
              student.PhotoPath = filename;
                
                var st = _repository.UpdateStudent(student);
                // return RedirectToAction("Details", new {id = st.StudentId});
                return RedirectToAction("Details", new { id = student.StudentId });
            }
            return View("Edit");
        }

        private string getFinleName(StudentEditViewModelClass model, string filename)
        {
            if (model.PhotoPath != null)
            {
                string uploads =
                    Path.Combine(
                        histingEnviroment.WebRootPath, "images"
                    );

                filename = Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName.ToString();
                string filePath = Path.Combine(uploads, filename);
                model.PhotoPath.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return filename;
        }
    }
}


//<staticContent>
//			<mimeMap fileExtension = "." mimeType="text/plain"/>
//		</staticContent>
//public JsonResult Index()
//{
//    return new JsonResult(_repository.GetStudents(1));
//    //return new JsonResult(new { ID=1,Name="sagar"});
//}


//public ViewResult Details()
//{
//    Student student = _repository.GetStudents(1);
//    //return View("../folderName/fileName",student);
//    //ViewData["NewTitle"] = "Student Information";
//    ViewBag.TitleNew = "Student Info";
//    return View(student);
//}