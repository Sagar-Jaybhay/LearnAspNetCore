using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnAspCore.Controllers
{
    public class ErrorController : Controller
    {


        public ErrorController(ILogger<ErrorController> logger)
        {
            this.LoggerObjetct = logger;
        }

        private ILogger<ErrorController> LoggerObjetct { get; set; }


        [Route("Error/{StatusCode}")]
        public IActionResult StatusCodeHandle(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessasge = $"I am having {statusCode}" +" Error code Message";
                    break;
                    
            }
            return View(statusCode);
        }


        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionfeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionfeature.Path+"\n";
            ViewBag.ExceptionMessage = exceptionfeature.Error.Message;
            ViewBag.StackTrace = exceptionfeature.Error.StackTrace;
            LoggerObjetct.LogError($"The Path is {exceptionfeature.Path} and Error Message {exceptionfeature.Error.Message} and StackTrace {exceptionfeature.Error.StackTrace}");

            return View();

        }

    }
}