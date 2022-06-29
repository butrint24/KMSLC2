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
    public class HomeWorkSubController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        public HomeWorkSubController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //GET studnepanel/homewroksub
        public async Task<IActionResult> Index(int p = 1)
        {
            var current_user = await userManager.GetUserAsync(User);
            var currentUserId = current_user.Id;
            var homeWorkSub = context.HomeWorkSubs.Include(x => x.AppUser)
                                                  .Where(x => x.StudentId == currentUserId)
                                                  .OrderByDescending(x => x.Order);

            return View(await homeWorkSub.ToListAsync());
        }

        //GET studenpanel/homeworksub/create
        public IActionResult Create() => View();

        //POST studenpanel/homeworksub/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomeWorkSub homeWorkSub)
        {     

            if (ModelState.IsValid)
            {
                homeWorkSub.Id = Guid.NewGuid();
                homeWorkSub.Order = DateTime.Now;

                ///<summary>
                ///Get Last Homework
                ///</summary>
                var lastItem = context.HomeWorks
                       .OrderByDescending(p => p.Order)
                       .FirstOrDefault();

                ///<summary>
                ///Get Homework Id
                ///</summary>
                var homeWorkId = lastItem.HomeWorkId;
                homeWorkSub.HomeworkId = homeWorkId.ToString();

                ///<summary>
                ///Get Homework Name
                ///</summary>
                var homeWorkName = lastItem.Title;
                homeWorkSub.HomeworkName = homeWorkName;

                ///<summary>
                ///Get Student Username
                ///</summary>
                var UserName = this.User.Identity.Name;
                homeWorkSub.StudentName = UserName;

                ///<summary>
                ///Get Logged in user prop
                ///</summary>
                var currentUser = await userManager.GetUserAsync(User);

                ///<summary>
                ///Get Teacher Id
                ///</summary>
                var teacherName = currentUser.TeacherName;
                homeWorkSub.TeacherId = teacherName;


                ///<summary>
                ///Get Student Id
                ///</summary>
                var studnetId = currentUser.Id;
                homeWorkSub.StudentId = studnetId;

                homeWorkSub.Grade = "0";

                context.Add(homeWorkSub);
                await context.SaveChangesAsync();

                TempData["Success"] = "Homework has been submitted!";

                return RedirectToAction("Index");
            }
            return View(homeWorkSub);
        }

        //GET student/homeworksub/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            HomeWorkSub homeworkSub = await context.HomeWorkSubs.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);

            if (homeworkSub == null)
                return NotFound();

            return View(homeworkSub);
        }

        //GET employeepanel/homeworks/edit/id
        public async Task<IActionResult> Edit(Guid? id)
        {
            HomeWorkSub homeworkSub = await context.HomeWorkSubs.FindAsync(id);

            if (homeworkSub == null)
                return NotFound();
                
            return View(homeworkSub);
        }

        //POST employeepanel/homeworks/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HomeWorkSub homeWork)
        {
            if (ModelState.IsValid)
            {
                context.Update(homeWork);
                await context.SaveChangesAsync();

                TempData["Success"] = "Homework submission has been edited!";

                return RedirectToAction("Edit", new { id = homeWork.Id });
            }

            return View(homeWork);
        }

        //GET studentpanel/Homeworksub/delete/id
        public async Task<IActionResult> Delete(Guid? id)
        {
            HomeWorkSub homeWorkSub = await context.HomeWorkSubs.FindAsync(id);

            if (homeWorkSub == null)
                TempData["Error"] = "This homework does not exist!";
                
            else
            {
                context.HomeWorkSubs.Remove(homeWorkSub);
                await context.SaveChangesAsync();

                TempData["Success"] = "This homework submission has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }
    }
}
