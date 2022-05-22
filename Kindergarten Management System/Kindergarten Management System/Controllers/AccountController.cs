using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Controllers
{
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

            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;


        // GET /account/login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = returnUrl
            };

            return View(login);
        }



        // Post /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, true);
                    if (result.Succeeded)



                    if (result.IsLockedOut)
                        ModelState.AddModelError("", "Your account is locked out try again later.");
                }
                ModelState.AddModelError("", "Login failed, wrong credentials.");
            }

            return View(login);
        }


        // GET /account/logout
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Redirect("/");
        }

        // GET /account/EmployeeEdit
        public async Task<IActionResult> EmployeeEdit()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            EmployeeEdit employeeEdit = new EmployeeEdit(appUser);

            return View(employeeEdit);
        }

        // Post /account/EmployeeEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeEdit(EmployeeEdit employeeEdit)
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
                }
                

            }

            return View();
        }
        // GET /account/StudentEdit
        public async Task<IActionResult> StudentEdit()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            StudentEdit studentEdit = new StudentEdit(appUser);

            return View(studentEdit);
        }


        // Post /account/StudentEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(StudentEdit studentEdit)
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            studentEdit.Image = appUser.StudentImage;
            if (ModelState.IsValid)
            {

                if (studentEdit.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnviroment.WebRootPath, "media/studentpic");


                    if (!string.Equals(studentEdit.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, studentEdit.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + studentEdit.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await studentEdit.ImageUpload.CopyToAsync(fs);

                    fs.Close();

                    studentEdit.Image = imageName;
                }
                appUser.FullName = studentEdit.FullName;
                appUser.BirthDate = studentEdit.BirthDate;
                appUser.LegalGuardian = studentEdit.LegalGuardian;
                appUser.PhoneNumber = studentEdit.ContactNumber;
                appUser.GuardianOccupation = studentEdit.GuardianOccupation;
                appUser.City = studentEdit.City;
                appUser.Gender = studentEdit.Gender;
                appUser.TeacherName = studentEdit.TeacherName;
                appUser.Email = studentEdit.Email;
                appUser.StudentImage = studentEdit.Image;

                if (studentEdit.Password != null)
                {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, studentEdit.Password);
                }

                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your information has been edited!";
                }


            }

            return View();
        }
    }
}
