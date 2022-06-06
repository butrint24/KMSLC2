using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApplicationsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        //GET admin/applications
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var applications = context.Applications.OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Applications.Count() / pageSize);

            return View(await applications.ToListAsync());
        }

        //GET admin/applications/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            Application application= await context.Applications.FirstOrDefaultAsync(x => x.ApplicationId == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        //Get Request /admin/applications/delete/id
        public async Task<IActionResult> Delete(Guid? id)
        {
            Application application = await context.Applications.FindAsync(id);

            if (application == null)
            {
                TempData["Error"] = "The application does not exist!";
            }
            else
            {
                if (!string.Equals(application.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/applicationpic");

                    string oldImagePath = Path.Combine(uploadsDir, application.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                context.Applications.Remove(application);
                await context.SaveChangesAsync();

                TempData["Success"] = "The application has been deleted!";
            }
            return RedirectToAction("Index");
        }
    }
}
