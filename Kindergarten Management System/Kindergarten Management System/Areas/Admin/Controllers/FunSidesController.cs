using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class FunSidesController : Controller
    {
        private readonly ApplicationDbContext context;
        public FunSidesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET admin/funsides
        public async Task<IActionResult> Index(int p = 1)
        {

            int pageSize = 3;
            var funSides = context.FunSides.Include(x => x.AppUser).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.FunSides.Count() / pageSize);


            return View(await funSides.ToListAsync());
        }
        //GET admin/funside/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            FunSide funside = await context.FunSides.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.FunSideId == id);

            if (funside == null)
            {
                return NotFound();
            }

            return View(funside);
        }
        //GET adminpanel/funsides/delete/id/
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