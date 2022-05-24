using Kindergarten_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
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

            int pageSize = 6;
            var homeWorks = context.HomeWorks.Include(x => x.AppUser).OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.HomeWorks.Count() / pageSize);


            return View(await homeWorks.ToListAsync());
        }
    }

}