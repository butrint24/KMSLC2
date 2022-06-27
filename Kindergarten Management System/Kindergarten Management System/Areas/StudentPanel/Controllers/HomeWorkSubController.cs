using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.StudentPanel.Controllers
{
    [Authorize(Roles = "Student")]
    [Area("StudentPanel")]
    public class HomeWorkSub : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        public HomeWorkSub(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //GET admin/funsides
        public async Task<IActionResult> Index(int p = 1)
        {

            var homeWorkSub = context.HomeWorkSubs.Include(x => x.AppUser).OrderByDescending(x => x.Order);

            return View(await homeWorkSub.ToListAsync());
        }


        //GET employeepnael/Funside/create
        public IActionResult Create() => View();

        //POST employeepnael/Funside/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.HomeWorkSub homeWorkSub, List<IFormFile> files)
        {     

            if (ModelState.IsValid)
            {
                homeWorkSub.Id = Guid.NewGuid();
                homeWorkSub.Order = DateTime.Now;

                var size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), formFile.FileName);
                        filePaths.Add(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                var lastItem = context.HomeWorks
                       .OrderByDescending(p => p.Order)
                       .FirstOrDefault();

                var homeWorkId = lastItem.Title;

                homeWorkSub.HomeworkId = homeWorkId;


                var UserName = this.User.Identity.Name;
                homeWorkSub.StudentName = UserName;

                var currentUser = await userManager.GetUserAsync(User);
                
                var teacherName = currentUser.TeacherName;
                homeWorkSub.TeacherId = teacherName;

                var studnetId = currentUser.Id;
                homeWorkSub.StudentId = studnetId;

                context.Add(homeWorkSub);
                await context.SaveChangesAsync();

                TempData["Success"] = "Homework has been submitted!";

                return RedirectToAction("Index");
            }
            return View(homeWorkSub);
        }

    }
}
