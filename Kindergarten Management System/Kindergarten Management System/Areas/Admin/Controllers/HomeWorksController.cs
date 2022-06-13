using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeWorksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET admin/HomeWorks
        public async Task<IActionResult> Index(int p = 1)
        {

            int pageSize = 3;
            var homeWorks = context.HomeWorks.Include(x => x.AppUser).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.HomeWorks.Count() / pageSize);


            return View(await homeWorks.ToListAsync());
        }
        //GET admin/homework/details/id
        public async Task<IActionResult> Details(Guid? id)
        {

            HomeWork homeWorks = await context.HomeWorks.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.HomeWorkId == id);

            if (homeWorks == null)
            {
                return NotFound();
            }

            return View(homeWorks);
        }
        //GET adminpanel/homeWork/delete/id
        public async Task<IActionResult> Delete(Guid? id)
        {
            HomeWork homeWork = await context.HomeWorks.FindAsync(id);

            if (homeWork == null)
            {
                TempData["Error"] = "This homework does not exist!";
            }
            else
            {
                context.HomeWorks.Remove(homeWork);
                await context.SaveChangesAsync();

                TempData["Success"] = "The homework has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }
    }

}
