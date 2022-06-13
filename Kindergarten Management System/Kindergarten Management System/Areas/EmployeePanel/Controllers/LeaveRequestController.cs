using AutoMapper;
using Kindergarten_Management_System.Contracts;
using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.EmployeePanel.Controllers
{
    [Authorize(Roles = "Employee")]
    [Area("EmployeePanel")]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocRepo;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepo,
            ILeaveTypeRepository leaveTypeRepo,
            ILeaveAllocationRepository leaveAllocRepo,
            IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context
            )
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocRepo = leaveAllocRepo;
            _mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public ActionResult MyLeave()
        {
            var employee = userManager.GetUserAsync(User).Result;
            var employeeid = employee.Id;
            var employeeAllocations = _leaveAllocRepo.GetLeaveAllocationsByEmployee(employeeid);
            var employeeRequests = _leaveRequestRepo.GetLeaveRequestsByEmployee(employeeid);

            var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationVM>>(employeeAllocations);
            var employeeRequestsModel = _mapper.Map<List<LeaveRequestVM>>(employeeRequests);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = employeeAllocationsModel,
                LeaveRequests = employeeRequestsModel
            };

            return View(model);
        }

        // GET: LeaveRequestController/Create
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypesItems = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });
            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypesItems
            };
            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model, LeaveRequest leaveRequest1)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _leaveTypeRepo.FindAll();
                var leaveTypesItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypesItems;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "error ne date - nuk mund te jete me e heret ose me e madhe se sot");
                    return View(model);
                }
                var employee = userManager.GetUserAsync(User).Result;
                var allocations = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(endDate.Date - startDate.Date).TotalDays;

                if (daysRequested > allocations.NumberOfDays)
                {
                    ModelState.AddModelError("", "error sepse nuk i keni keto dit te lejuara");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess = _leaveRequestRepo.CreateT(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "error ne leaverequest duke ber submit your record");
                    return View(model);
                }

                return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error f1" + ex);
                return View(model);
            }
        }
        public ActionResult CancelRequest(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            if (leaveRequest.Approved == null)
            {
                _leaveRequestRepo.DeleteT(leaveRequest);
                TempData["Success"] = "Your leave has been cancelled";
                return RedirectToAction("MyLeave");
            }
            else
            {
                TempData["Error"] = "You can't remove after leave is approved or rejected ";
                return RedirectToAction("MyLeave");
            }
        }
    }
}
