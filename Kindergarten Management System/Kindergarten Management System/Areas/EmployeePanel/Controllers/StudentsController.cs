using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class StudentsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;

        public StudentsController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;

        }

        //GET EmployeePanel/Students
        public async Task<IActionResult> Index(int p = 1)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int pageSize = 2;
            var users = context.Users.Where(x => x.TeacherName == userId).OrderByDescending(x => x.Order)
                                     .Skip((p - 1) * pageSize)
                                     .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Users.Where(x => x.TeacherName == userId).Count() / pageSize);

            return View(await users.ToListAsync());
        }

        //GET /admin/student/details/5
        public async Task<IActionResult> Details(Student student, string id)
        {
            AppUser appUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            Student studentDetails = new Student(appUser);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(studentDetails);
        }
    }
}
