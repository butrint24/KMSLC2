using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
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
    }
}
