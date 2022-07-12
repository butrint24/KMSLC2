using AutoMapper;
using Kindergarten_Management_System.Contracts;
using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocRepo;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IEmailSender emailSender;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepo,
            ILeaveTypeRepository leaveTypeRepo,
            ILeaveAllocationRepository leaveAllocRepo,
            IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender
            )
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocRepo = leaveAllocRepo;
            _mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.emailSender = emailSender;
        }
        // GET: LeaveRequestController
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequestes = leaveRequestsModel.Count,
                ApprovedRequestes = leaveRequestsModel.Count(q => q.Approved == true),
                PendingRequestes = leaveRequestsModel.Count(q => q.Approved == null),
                RejectedRequestes = leaveRequestsModel.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestsModel
            };
            return View(model);
        }

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            var model = _mapper.Map<LeaveRequestVM>(leaveRequest);
            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var user = userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                var employeeid = leaveRequest.RequestingEmployeeId;
                var employeeEmail = leaveRequest.RequestingEmployeeEmail;
                var employeeName = leaveRequest.RequestingEmployee.FullName;
                var startDate = leaveRequest.StartDate;
                var endDate = leaveRequest.EndDate;
                var approvedBy = leaveRequest.ApprovedBy;
                var leaveTypeId = leaveRequest.LeaveTypeId;
                var allocation = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employeeid, leaveTypeId);
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;
                if (allocation.NumberOfDays == 0)
                {
                    ModelState.AddModelError("", "i keni shfrytezuar ditet...");
                }
                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                emailSender.SendEmailAsync(employeeEmail, "Leave Request",
                    $"Dear " + employeeName + " - Your requested leave from " + startDate + " to " + endDate + " has been accepted." +
                    "You requested a total of " + daysRequested + " days off. Please do not reply to this message.Replies to this message are routed to an unmonitored mailbox.If you have questions please contact : kindergartenmanagement@outlook.com");


                _leaveRequestRepo.UpdateT(leaveRequest);
                _leaveAllocRepo.UpdateT(allocation);

                
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index) + ex);
            }
        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                var employeeEmail = leaveRequest.RequestingEmployeeEmail;
                var startDate = leaveRequest.StartDate;
                var endDate = leaveRequest.EndDate;
                var employeeName = leaveRequest.RequestingEmployee.FullName;
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                emailSender.SendEmailAsync(employeeEmail, "Leave Request", 
                    $"Dear " +  employeeName + " - Your requested leave from " + startDate + " to " + endDate + " has been refused."+
"Please do not reply to this message.Replies to this message are routed to an unmonitored mailbox.If you have questions please contact : kindergartenmanagement@outlook.com");

                _leaveRequestRepo.UpdateT(leaveRequest);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index) + ex);
            }
        }
    }
}
