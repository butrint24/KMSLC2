using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApplicationsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        //GET Applications
        public IActionResult Index() => View();

        //POST Applications
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Application application)
        {
            if (ModelState.IsValid)
            {
                application.ApplicationId = Guid.NewGuid();
                application.Order = DateTime.Now;

                var pNumber = await context.Applications.FirstOrDefaultAsync(x => x.PersonalNumber == application.PersonalNumber);
                if (pNumber != null)
                {
                    ModelState.AddModelError("", "This Application already exists!");

                    return View(application);
                }

                string imageName = "noimage.png";
                if (application.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/applicationpic");
                    imageName = Guid.NewGuid().ToString() + "_" + application.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await application.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                application.Image = imageName;

                context.Add(application);
                await context.SaveChangesAsync();

                TempData["Success"] = "Application has been sent!";

                return RedirectToAction("Index", "Home");
            }
            return View(application);
        }
    }
}
