using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext context;
        public ContactsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET admin/contacts
        public async Task<IActionResult> Index()
        {

            var contacts = context.Contacts.OrderByDescending(x => x.Order);
                                     
            return View(await contacts.ToListAsync());
        }

        //GET admin/contacts/details/id
        public async Task<IActionResult> Details(Guid? id)
        {
            Contact contact = await context.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        //GET admin/contacts/delete/id
        public async Task<IActionResult> Delete(Guid? id)
        {
            Contact contact = await context.Contacts.FindAsync(id);

            if (contact == null)
            {
                TempData["Error"] = "The briefing does not exist!";
            }
            else
            {
                context.Contacts.Remove(contact);
                await context.SaveChangesAsync();

                TempData["Success"] = "The briefing has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }
    }
}
