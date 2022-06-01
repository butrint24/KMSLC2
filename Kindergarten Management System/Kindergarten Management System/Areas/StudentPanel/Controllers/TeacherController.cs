using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.StudentPanel.Controllers
{
    [Area("StudentPanel")]
    public class TeacherController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;

        public TeacherController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;

        }

        //GET StudentPanel/Teacher
        public async Task<IActionResult> Index(Employee employee, int p = 1)
        {
            var current_user = await userManager.GetUserAsync(User);
            var getTeacherName = current_user.TeacherName;
            int pageSize = 6;
            var users = context.Users.Where(x => x.Id == getTeacherName).OrderByDescending(x => x.Order)
                                     .Skip((p - 1) * pageSize)
                                     .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Users.Where(x => x.Id == getTeacherName).Count() / pageSize);

            return View(await users.ToListAsync());
        }
    }
}
