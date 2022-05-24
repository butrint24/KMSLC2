using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.StudentPanel.Controllers
{
    [Area("StudentPanel")]
    public class FunSidesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        public FunSidesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        //GET admin/funsides
        public async Task<IActionResult> Index(Student student, int p = 1)
        {

            var current_user = await userManager.GetUserAsync(User);
            var getTeacherName = current_user.TeacherName;
            int pageSize = 6;
            var funSides = context.FunSides.Include(x => x.AppUser).Where(x => x.TeacherId == getTeacherName).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.FunSides.Where(x => x.TeacherId == getTeacherName).Count() / pageSize);

            return View(await funSides.ToListAsync());
        }

        //GET admin/funside/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            FunSide funside = await context.FunSides.FirstOrDefaultAsync(x => x.FunSideId == id);

            if (funside == null)
            {
                return NotFound();
            }

            return View(funside);
        }
    }
}
