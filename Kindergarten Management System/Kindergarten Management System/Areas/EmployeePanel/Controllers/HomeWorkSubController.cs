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

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Authorize(Roles = "Employee")]
    [Area("EmployeePanel")]
    public class HomeWorkSubController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        public HomeWorkSubController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //GET EmployeePanel/homewroksub
        public async Task<IActionResult> Index(int p = 1)
        {
            var current_user = await userManager.GetUserAsync(User);
            var currentUserId = current_user.Id;

            var homeWorkSub = context.HomeWorkSubs.Include(x => x.AppUser)
                                                  .Where(x => x.TeacherId == currentUserId)
                                                  .OrderByDescending(x => x.Order);

            return View(await homeWorkSub.ToListAsync());
        }

        //GET EmployeePanel/homeworksub/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            HomeWorkSub homeworkSub = await context.HomeWorkSubs.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);

            if (homeworkSub == null)
            {
                return NotFound();
            }

            return View(homeworkSub);
        }

        //GET EmployeePanel/homeworks/edit/id
        public async Task<IActionResult> Edit(Guid? id)
        {
            HomeWorkSub homeworkSub = await context.HomeWorkSubs.FindAsync(id);

            if (homeworkSub == null)
            {
                return NotFound();
            }
            return View(homeworkSub);
        }

        //POST EmployeePanel/homeworks/edit/id
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
    }
}
