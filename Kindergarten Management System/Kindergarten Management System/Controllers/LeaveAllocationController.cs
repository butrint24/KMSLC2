using AutoMapper;
using Kindergarten_Management_System.Contracts;
using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Controllers
{
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaverepo;
        private readonly ILeaveAllocationRepository _leaveallocationrepo;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public LeaveAllocationController(
            ILeaveTypeRepository leaverepo,
            IMapper mapper,
            ILeaveAllocationRepository leaveallocationrepo,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _leaverepo = leaverepo;
            _mapper = mapper;
            _leaveallocationrepo = leaveallocationrepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        // GET: LeaveAllocationController
        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var leavetypes = _leaverepo.FindAll().ToList();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            var model = new CreateLeaveAllocationVM
            {
                LeaveType = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }

        public ActionResult SetLeave(int id)
        {
            var leavetype = _leaverepo.FindById(id);
            var employee = userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var emp in employee)
            {
                if (_leaveallocationrepo.CheckAllocation(id, emp.Id))
                    continue;
                var allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    employeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leavetype.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                _leaveallocationrepo.CreateT(leaveallocation);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ListEmployees()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Id"] = userId;
            var employees = userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<AppUserVM>>(employees);
            return View(model);
        }

        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            var employee = _mapper.Map<AppUserVM>(userManager.FindByIdAsync(id).Result);
            var allocation = _mapper.Map<List<LeaveAllocationVM>>(_leaveallocationrepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewAllocationsVM
            {
                Employee = employee,
                LeaveAllocations = allocation
            };
            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}