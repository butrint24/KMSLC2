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
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController
            (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.webHostEnvironment = webHostEnvironment;
        }

        //Get /Account/RegisterStudent
        public IActionResult RegisterStudent()
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "FullName", "FullName");
            return View();
        } 


        //POST /account/RegisterStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(Student student)
        {

            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "FullName", "FullName");

            //Check if Email exists in user db
            var email = await userManager.FindByEmailAsync(student.Email);
            if (email != null)
            {
                ModelState.AddModelError("", "The email already exist");
                return View(student);
            }


            if (ModelState.IsValid)
            {

                string imageName = "noimage.png";
                if (student.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/studentpic");

                    imageName = Guid.NewGuid().ToString() + "_" + student.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await student.ImageUpload.CopyToAsync(fs);

                    fs.Close();


                }
                student.Image = imageName;


                AppUser appUser = new AppUser
                {
                    FullName = student.FullName,
                    BirthDate = student.BirthDate,
                    LegalGuardian = student.LegalGuardian,
                    PhoneNumber = student.ContactNumber,
                    GuardianOccupation = student.GuardianOccupation,
                    City = student.City,
                    Gender = student.Gender,
                    UserName = student.UserName,
                    Email = student.Email,
                    StudentImage = student.Image,
                    IsEmployee = false,
                    Order = DateTime.Now,
                    TeacherName = student.TeacherName
                };

                IdentityResult result = await userManager.CreateAsync(appUser, student.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(student);

        }



        //Get /Account/RegisterEmployee
        public IActionResult RegisterEmployee() => View();


        //POST /account/RegisterEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEmployee(Employee employee)
        {

            //Check if Email exists in user db
            var email = await userManager.FindByEmailAsync(employee.Email);
            if (email != null)
            {
                ModelState.AddModelError("", "The email already exist");
                return View(employee);
            }


            if (ModelState.IsValid)
            {

                string imageName = "noimage.png";
                if (employee.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/employeepic");

                    imageName = Guid.NewGuid().ToString() + "_" + employee.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await employee.ImageUpload.CopyToAsync(fs);

                    fs.Close();


                }
                employee.Image = imageName;


                AppUser appUser = new AppUser
                {
                    FullName = employee.FullName,
                    BirthDate = employee.BirthDate,
                    PersonalNumber = employee.PersonalNumber,
                    Title = employee.Title,
                    Bio = employee.Bio,
                    PhoneNumber = employee.ContactNumber,
                    City = employee.City,
                    Gender = employee.Gender,
                    UserName = employee.UserName,
                    Email = employee.Email,
                    StudentImage = employee.Image,
                    IsEmployee = true,
                    Order = DateTime.Now,
                };

                IdentityResult result = await userManager.CreateAsync(appUser, employee.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(employee);

        }




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
                        return Redirect(login.ReturnUrl ?? "/");

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
    }
}
