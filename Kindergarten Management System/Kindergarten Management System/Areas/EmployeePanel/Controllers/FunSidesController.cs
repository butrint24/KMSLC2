using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Authorize(Roles = "Employee")]
    [Area("EmployeePanel")]
    public class FunSidesController : Controller
    {
        private readonly ApplicationDbContext context;
        public FunSidesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET employeepnael/funsides
        public async Task<IActionResult> Index(int p = 1)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int pageSize = 3;
            var funSides = context.FunSides.Where(x => x.TeacherId == userId ).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.FunSides.Where(x => x.TeacherId == userId).Count() / pageSize);

            return View(await funSides.ToListAsync());
        }
        //GET employeepnael/Funside/create
        public IActionResult Create() => View();

        //POST employeepnael/Funside/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FunSide funside)
        {
            if (ModelState.IsValid)
            {
                funside.FunSideId = Guid.NewGuid();
                funside.Slug = funside.Title.ToLower().Replace(" ", "-");
                funside.Order = DateTime.Now;
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                funside.TeacherId = userId;

                var slug = await context.FunSides.FirstOrDefaultAsync(x => x.Slug == funside.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The FunSide already exists!");

                    return View(funside);
                }

                context.Add(funside);
                await context.SaveChangesAsync();

                TempData["Success"] = "FunSide has been added!";

                return RedirectToAction("Index");
            }
            return View(funside);
        }

        //GET employeepnael/funside/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            FunSide funside = await context.FunSides.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.FunSideId == id);

            if (funside == null)
            {
                return NotFound();
            }

            return View(funside);
        }

        //GET employeepanel/funsides/edit/id
        public async Task<IActionResult> Edit(Guid? id)
        {
            FunSide funSide = await context.FunSides.FindAsync(id);

            if (funSide == null)
            {
                return NotFound();
            }
            return View(funSide);
        }


        //POST employeepanel/funsides/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FunSide funSide)
        {
            if (ModelState.IsValid)
            {
                funSide.Slug = funSide.Title.ToLower().Replace(" ", "-");

                var slug = await context.FunSides.Where(x => x.FunSideId != funSide.FunSideId).FirstOrDefaultAsync(x => x.Slug == funSide.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The funside already exists!");

                    return View(funSide);
                }

                context.Update(funSide);
                await context.SaveChangesAsync();

                TempData["Success"] = "Funside has been edited!";

                return RedirectToAction("Edit", new { id = funSide.FunSideId });
            }

            return View(funSide);
        }

        //GET employeepanel/funsides/delete/id/
        public async Task<IActionResult> Delete(Guid? id)
        {
            FunSide funSide = await context.FunSides.FindAsync(id);

            if (funSide == null)
            {
                TempData["Error"] = "This funside does not exist!";
            }
            else
            {
                context.FunSides.Remove(funSide);
                await context.SaveChangesAsync();

                TempData["Success"] = "The funside has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

    }
}