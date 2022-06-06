using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BriefingsController : Controller
    {
        private readonly ApplicationDbContext context;
        public BriefingsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET admin/briefings
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var briefings = context.Briefings.OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Briefings.Count() / pageSize);

            return View(await briefings.ToListAsync());
        }


        //GET admin/briefings/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            Briefing briefing = await context.Briefings.FirstOrDefaultAsync(x => x.BriefingId == id);

            if (briefing == null)
            {
                return NotFound();
            }

            return View(briefing);
        }

        //GET admin/briefings/create
        public IActionResult Create() => View();

        //POST admin/briefing/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Briefing briefing)
        {
            if(ModelState.IsValid)
            {
                briefing.BriefingId = Guid.NewGuid();
                briefing.Slug = briefing.Title.ToLower().Replace(" ", "-");
                briefing.Order = DateTime.Now;

                var slug = await context.Briefings.FirstOrDefaultAsync(x => x.Slug == briefing.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "The briefing already exists!");

                    return View(briefing);
                }

                context.Add(briefing);
                await context.SaveChangesAsync();

                TempData["Success"] = "Briefing has been added!";
                
                return RedirectToAction("Index");
            }
            return View(briefing);
        }

        //GET admin/briefings/edit/id
        public async Task<IActionResult> Edit(Guid? id)
        {
            Briefing briefing = await context.Briefings.FindAsync(id);

            if(briefing == null)
            {
                return NotFound();
            }
            return View(briefing);
        }


        //POST admin/briefings/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Briefing briefing)
        {
            if(ModelState.IsValid)
            {
                briefing.Slug = briefing.Title.ToLower().Replace(" ", "-");

                var slug = await context.Briefings.Where(x => x.BriefingId != briefing.BriefingId).FirstOrDefaultAsync(x => x.Slug == briefing.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The briefing already exists!");

                    return View(briefing);
                }

                context.Update(briefing);
                await context.SaveChangesAsync();

                TempData["Success"] = "Briefing has been edited!";

                return RedirectToAction("Edit", new { id = briefing.BriefingId });
            }

            return View(briefing);
        }
        
        //GET admin/briefings/delete/id
        public async Task<IActionResult> Delete(Guid? id)
        {
            Briefing briefing = await context.Briefings.FindAsync(id);

            if (briefing == null)
            {
                TempData["Error"] = "The briefing does not exist!";
            }
            else
            {
                context.Briefings.Remove(briefing);
                await context.SaveChangesAsync();

                TempData["Success"] = "The briefing has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

    }
}
