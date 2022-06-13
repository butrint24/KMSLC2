using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Authorize(Roles = "Employee")]
    [Area("EmployeePanel")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly IWebHostEnvironment webHostEnviroment;

        public AccountController
            (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher,
            IWebHostEnvironment webHostEnviroment
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.webHostEnviroment = webHostEnviroment;
        }

        // GET /account/EmployeeEdit
        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            EmployeeEdit employeeEdit = new EmployeeEdit(appUser);

            return View(employeeEdit);
        }


        //GET /admin/employee/details/5
        public async Task<IActionResult> Details(Employee employee, string id)
        {

            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);


            if (User.IsInRole("Employee"))
            {

                Employee employeeDetails = new Employee(appUser);
                if (appUser == null)
                {
                    return NotFound();
                }


                return View(employeeDetails);
            }

            return BadRequest();
        }

        // Post /account/EmployeeEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEdit employeeEdit)
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            employeeEdit.Image = appUser.TeacherImage;
            if (ModelState.IsValid)
            {

                if (employeeEdit.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnviroment.WebRootPath, "media/employeepic");


                    if (!string.Equals(employeeEdit.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, employeeEdit.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + employeeEdit.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await employeeEdit.ImageUpload.CopyToAsync(fs);

                    fs.Close();

                    employeeEdit.Image = imageName;
                }
                appUser.FullName = employeeEdit.FullName;
                appUser.BirthDate = employeeEdit.BirthDate;
                appUser.PersonalNumber = employeeEdit.PersonalNumber;
                appUser.City = employeeEdit.City;
                appUser.Gender = employeeEdit.Gender;
                appUser.Title = employeeEdit.Title;
                appUser.Bio = employeeEdit.Bio;
                appUser.Email = employeeEdit.Email;
                appUser.TeacherImage = employeeEdit.Image;
                appUser.PhoneNumber = employeeEdit.ContactNumber;

                if (employeeEdit.Password != null)
                {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, employeeEdit.Password);
                }

                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your information has been edited!";
                    return RedirectToAction("Details");
                }
                


            }
            return View();
        }
    }
}
