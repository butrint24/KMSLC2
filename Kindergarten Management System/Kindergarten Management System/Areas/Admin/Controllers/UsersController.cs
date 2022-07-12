using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnviroment;
        private IPasswordHasher<AppUser> passwordHasher;

        public UsersController(UserManager<AppUser> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnviroment, IPasswordHasher<AppUser> passwordHasher)
        {
            this.userManager = userManager;
            this.context = context;
            this.webHostEnviroment = webHostEnviroment;
            this.passwordHasher = passwordHasher;

        }

        //GET admin/users
        public async Task<IActionResult> Index(int p = 1)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            int pageSize = 6;
            var users = context.Users.OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Users.Count() / pageSize);

            return View(await users.ToListAsync());
        }


        //GET admin/users/students
        public async Task<IActionResult> Students(int p = 1)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            int pageSize = 3;
            var users = context.Users.OrderByDescending(x => x.Order)
                                     .Where(x => x.IsEmployee == false)
                                     .Skip((p - 1) * pageSize)
                                     .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Users.Where(x => x.IsEmployee == false).Count() / pageSize);

            return View(await users.ToListAsync());
        }

        //GET admin/users/employees
        public async Task<IActionResult> Employees(int p = 1)
        {
            int pageSize = 3;
            var users = context.Users.OrderByDescending(x => x.Order)
                                     .Where(x => x.IsEmployee == true)
                                     .Skip((p - 1) * pageSize)
                                     .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Users.Where(x => x.IsEmployee == true).Count() / pageSize);

            return View(await users.ToListAsync());
        }

        //GET /admin/employee/details/5
        public async Task<IActionResult> EmployeeDetails(Employee employee, string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            Employee employeeDetails = new Employee(appUser);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(employeeDetails);
        }

        //GET /admin/student/details/5
        public async Task<IActionResult> StudentDetails(Student student, string id)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            Student studentDetails = new Student(appUser);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(studentDetails);
        }


        // GET /account/AdminEmployeeEdit
        public async Task<IActionResult> AdminEmployeeEdit(string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            AdminEmployeeEdit adminEmployeeEdit = new AdminEmployeeEdit(appUser);

            return View(adminEmployeeEdit);
        }

        // Post /account/AdminEmployeeEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEmployeeEdit(AdminEmployeeEdit adminEmployeeEdit, string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            adminEmployeeEdit.Image = appUser.TeacherImage;
            if (ModelState.IsValid)
            {

                if (adminEmployeeEdit.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnviroment.WebRootPath, "media/employeepic");


                    if (!string.Equals(adminEmployeeEdit.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, adminEmployeeEdit.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + adminEmployeeEdit.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await adminEmployeeEdit.ImageUpload.CopyToAsync(fs);

                    fs.Close();

                    adminEmployeeEdit.Image = imageName;
                }
                appUser.FullName = adminEmployeeEdit.FullName;
                appUser.BirthDate = adminEmployeeEdit.BirthDate;
                appUser.PersonalNumber = adminEmployeeEdit.PersonalNumber;
                appUser.City = adminEmployeeEdit.City;
                appUser.Gender = adminEmployeeEdit.Gender;
                appUser.Title = adminEmployeeEdit.Title;
                appUser.Bio = adminEmployeeEdit.Bio;
                appUser.Email = adminEmployeeEdit.Email;
                appUser.TeacherImage = adminEmployeeEdit.Image;
                appUser.PhoneNumber = adminEmployeeEdit.ContactNumber;
                appUser.Status = adminEmployeeEdit.Status;

                if (adminEmployeeEdit.Password != null)
                {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, adminEmployeeEdit.Password);
                }

                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your information has been edited!";
                }


            }

            return View();
        }
        // GET /account/AdminStudentEdit
        public async Task<IActionResult> AdminStudentEdit(string id)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            AdminStudentEdit adminStudentEdit = new AdminStudentEdit(appUser);

            return View(adminStudentEdit);
        }

        // Post /account/AdminStudentEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminStudentEdit(AdminStudentEdit adminStudentEdit, string id)
        {
            ViewBag.TeacherName = new SelectList(context.Users.Where(x => x.IsEmployee == true), "Id", "FullName");
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            adminStudentEdit.Image = appUser.StudentImage;
            if (ModelState.IsValid)
            {

                if (adminStudentEdit.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnviroment.WebRootPath, "media/studentpic");


                    if (!string.Equals(adminStudentEdit.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, adminStudentEdit.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + adminStudentEdit.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await adminStudentEdit.ImageUpload.CopyToAsync(fs);

                    fs.Close();

                    adminStudentEdit.Image = imageName;
                }
                    appUser.FullName = adminStudentEdit.FullName;
                    appUser.BirthDate = adminStudentEdit.BirthDate;
                    appUser.LegalGuardian = adminStudentEdit.LegalGuardian;
                    appUser.PhoneNumber = adminStudentEdit.ContactNumber;
                    appUser.GuardianOccupation = adminStudentEdit.GuardianOccupation;
                    appUser.City = adminStudentEdit.City;
                    appUser.Gender = adminStudentEdit.Gender;
                    appUser.UserName = adminStudentEdit.UserName;
                    appUser.Email = adminStudentEdit.Email;
                    appUser.StudentImage = adminStudentEdit.Image;
                    appUser.TeacherName = adminStudentEdit.TeacherName;


                if (adminStudentEdit.Password != null)
                {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, adminStudentEdit.Password);
                }

                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your information has been edited!";
                }


            }

            return View();
        }

        //Get Request /admin/users/Employeedelete/id
        public async Task<IActionResult> EmployeeDelete(AdminEmployeeEdit user, string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            AdminEmployeeEdit employeeDelete = new AdminEmployeeEdit(appUser);

            if (employeeDelete == null)
            {
                TempData["Error"] = "The user does not exist!";
            }
            else
            {

                await userManager.DeleteAsync(appUser);

                TempData["Success"] = "The user has been deleted!";
            }
            return RedirectToAction("Index");
        }

        //Get Request /admin/users/StudentDelete/id
        public async Task<IActionResult> StudentDelete(AdminStudentEdit user, string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            AdminStudentEdit studentDelete = new AdminStudentEdit(appUser);

            if (studentDelete == null)
            {
                TempData["Error"] = "The user does not exist!";
            }
            else
            {

                await userManager.DeleteAsync(appUser);

                TempData["Success"] = "The user has been deleted!";
            }
            return RedirectToAction("Index");
        }
    }
}
