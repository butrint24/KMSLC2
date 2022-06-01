using Kindergarten_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext context;
        public ContactsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET admin/contacts
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var contacts = context.Contacts.OrderByDescending(x => x.Order)
                                      .Skip((p - 1) * pageSize)
                                      .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Contacts.Count() / pageSize);

            return View(await contacts.ToListAsync());
        }
    }
}
