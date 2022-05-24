using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kindergarten_Management_System.Areas.StudentPanel.Controllers
{
    [Area("StudentPanel")]
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        public HomeWorksController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //GET student/homework
        public async Task<IActionResult> Index(Student student, int p = 1)
        {

            var current_user = await userManager.GetUserAsync(User);
            var getTeacherName = current_user.TeacherName;
            int pageSize = 6;
            var homeWorks = context.HomeWorks.Include(x => x.AppUser).Where(x => x.TeacherId == getTeacherName).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.HomeWorks.Where(x => x.TeacherId == getTeacherName).Count() / pageSize);

            return View(await homeWorks.ToListAsync());
        }

    }
}
