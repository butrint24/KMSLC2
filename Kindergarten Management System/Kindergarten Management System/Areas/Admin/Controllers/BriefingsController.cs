using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BriefingsController : Controller
    {
        public IActionResult Index() => View();
       
    }
}
