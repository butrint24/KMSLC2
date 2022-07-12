using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailSender emailSender;

        public AccountController
            (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.webHostEnvironment = webHostEnvironment;
            this.emailSender = emailSender;
        }

        //Get /Account/RegisterStudent
        public IActionResult RegisterStudent()
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            return View();
        } 


        //POST /account/RegisterStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(Student student)
        {

            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");

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
                IdentityResult creatUser = await userManager.CreateAsync(appUser, student.Password);
                IdentityResult result = await userManager.AddToRoleAsync(appUser, "Student");
                if (result.Succeeded)
                {
                    await emailSender.SendEmailAsync(appUser.Email, "Welcome to Kindergarten Management System", 
                        $"Dear " + appUser.FullName +
                        " -  We are all really excited to welcome you to KinderGarten Management System!"+
                        "Please do not reply to this message. Replies to this message are routed to an unmonitored mailbox. If you have questions please contact us : kindergartenmanagement@outlook.com");
                    return RedirectToAction("Students", "Users", new { area = "Admin" });
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
                    TeacherImage = employee.Image,
                    IsEmployee = true,
                    Order = DateTime.Now,
                    Status = true
                };

                IdentityResult result = await userManager.CreateAsync(appUser, employee.Password);
                IdentityResult assignToRole = await userManager.AddToRoleAsync(appUser, "Employee");
                if (result.Succeeded)
                {
                    await emailSender.SendEmailAsync(appUser.Email, "Welcome to Kindergarten Management System",
                        $"Dear " + appUser.FullName +
                        " -  We are all really excited to welcome you to our team! We expect you to be in our offices by time and our dress code is business casual." +
                        "" +
                        " * Remember you need to bring your ID at office at the first day of work to get your School ID. " +
                        "" +
                        "Please do not reply to this message. Replies to this message are routed to an unmonitored mailbox. If you have questions please contact us : kindergartenmanagement@outlook.com");
                    return RedirectToAction("Employees", "Users", new { area = "Admin"});
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

    }
}
