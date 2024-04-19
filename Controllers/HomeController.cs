using EmployeeMangement.Models;
using EmployeeMangement.Security;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Controllers
{
    
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnviroment;
        private readonly ILogger logger;
        private readonly IDataProtector protector;

        [Obsolete]
        public HomeController(IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnviroment,
            ILogger<HomeController> logger,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
         
            
            _employeeRepository = employeeRepository;
            this.hostingEnviroment = hostingEnviroment;
            this.logger = logger;
            protector = dataProtectionProvider
                .CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]

        public ViewResult  Index()
        {
            var model = _employeeRepository.GetAllEmployee()
                .Select(e =>
                {
                    e.EncryptedId = protector.Protect(e.Id.ToString());
                    return e;
                });

            return View(model) ;
        }
       [AllowAnonymous]
        public ViewResult  Details(string id)
        {

            // throw new Exception("Error in Details View");
            logger.LogTrace("Trace log");
            logger.LogInformation("Information log");
            logger.LogDebug("Debug log");
            logger.LogWarning("Warning log");
            logger.LogError("error log");
            logger.LogCritical("Critical log");

            int employeeId = Convert.ToInt32(protector.Unprotect(id));
            Employee employee = _employeeRepository.GetEmployee(employeeId);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", employeeId);
            }
            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
                Employee = employee,
                pagetitle = "Employee Details"


            };
           
          
            return View(homeDetailsViewModels);
        }
        //model binding
        [HttpGet]
        [AllowAnonymous]
      
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        
        public IActionResult Create(EmployeeCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);
                Employee newemployee = new Employee
                {
                    Name=model.Name,
                    Email=model.Email,
                    Department=model.Department,
                    PhotoPath= uniqueFileName

                };
                _employeeRepository.Add(newemployee);
               return RedirectToAction("details", new { id = newemployee.Id });
            }
            return View();
        }
        [HttpGet]
        
        public ActionResult Edit( int id)
        {
          

            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEdieViewModel employeeEdieViewModel = new EmployeeEdieViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEdieViewModel);
        }

        [HttpPost]
        [Obsolete]
    
        public IActionResult Edit(EmployeeEdieViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photos != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                  string filePath =Path.Combine(hostingEnviroment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadFile(model);
                }
               
                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        [Obsolete]
        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(hostingEnviroment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                   
                }
            }

            return uniqueFileName;
        }
    }
}
