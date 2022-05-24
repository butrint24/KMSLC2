using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.StudentPanel.Controllers
{
    [Area("StudentPanel")]
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


        //GET /admin/student/details/5
        public async Task<IActionResult> Details(Student student, string id)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);


            if (User.IsInRole("Student"))
            {

                Student studentDetails = new Student(appUser);
                if (appUser == null)
                {
                    return NotFound();
                }


                return View(studentDetails);
            }

            return BadRequest();
        }

        // GET /account/StudentEdit
        public async Task<IActionResult> Edit()
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            StudentEdit studentEdit = new StudentEdit(appUser);

            return View(studentEdit);
        }


        // Post /account/StudentEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentEdit studentEdit)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
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
                    return RedirectToAction("Details");
                }


            }

            return View();
        }
    }
}
