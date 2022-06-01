using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeWorksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET employeepanel/homeworks
        public async Task<IActionResult> Index(int p = 1)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int pageSize = 6;
            var homeWorks = context.HomeWorks.Where(x => x.TeacherId == userId).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.HomeWorks.Where(x => x.TeacherId == userId).Count() / pageSize);

            return View(await homeWorks.ToListAsync());
        }
        //GET employeepanel/HomeWork/Create
        public IActionResult Create() => View();

        //POST employeepanel/HomeWork/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomeWork homeWorks)
        {
            if (ModelState.IsValid)
            {
                homeWorks.HomeWorkId = Guid.NewGuid();
                homeWorks.Slug = homeWorks.Title.ToLower().Replace(" ", "-");
                homeWorks.Order = DateTime.Now;
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                homeWorks.TeacherId = userId;

                var slug = await context.HomeWorks.FirstOrDefaultAsync(x => x.Slug == homeWorks.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Home Work already exists!");

                    return View(homeWorks);
                }

                context.Add(homeWorks);
                await context.SaveChangesAsync();

                TempData["Success"] = "Home Work has been added!";

                return RedirectToAction("Index");
            }
            return View(homeWorks);
        }

        //GET employeepnael/homeworks/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            HomeWork homeWork = await context.HomeWorks.FirstOrDefaultAsync(x => x.HomeWorkId == id);

            if (homeWork == null)
            {
                return NotFound();
            }

            return View(homeWork);
        }

        //GET employeepanel/homeworks/edit/id
        public async Task<IActionResult> Edit(Guid? id)
        {
            HomeWork homeWork = await context.HomeWorks.FindAsync(id);

            if (homeWork == null)
            {
                return NotFound();
            }
            return View(homeWork);
        }


        //POST employeepanel/homeworks/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HomeWork homeWork)
        {
            if (ModelState.IsValid)
            {
                homeWork.Slug = homeWork.Title.ToLower().Replace(" ", "-");

                var slug = await context.HomeWorks.Where(x => x.HomeWorkId != homeWork.HomeWorkId).FirstOrDefaultAsync(x => x.Slug == homeWork.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "This homework already exists!");

                    return View(homeWork);
                }

                context.Update(homeWork);
                await context.SaveChangesAsync();

                TempData["Success"] = "Homework has been edited!";

                return RedirectToAction("Edit", new { id = homeWork.HomeWorkId });
            }

            return View(homeWork);
        }
        //GET employeepanel/Homework/delete/id
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

                TempData["Success"] = "This homework has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

    }
}
