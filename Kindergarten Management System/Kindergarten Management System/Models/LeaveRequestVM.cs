using Kindergarten_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Models
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }

        public AppUserVM RequestingEmployee { get; set; }

        public string RequestingEmployeeId { get; set; }
        [Required]
        [DataType(DataType.Date)]

        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]

        public DateTime EndDate { get; set; }

        public LeaveTypeVM LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime DateActioned { get; set; }

        public bool? Approved { get; set; }


        public AppUserVM ApprovedBy { get; set; }

        public string ApprovedById { get; set; }
        public bool Cancelled { get; set; }
    }

    public class AdminLeaveRequestViewVM
    {
        public int TotalRequestes { get; set; }

        public int ApprovedRequestes { get; set; }

        public int PendingRequestes { get; set; }

        public int RejectedRequestes { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }

    public class CreateLeaveRequestVM
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EmployeeLeaveCreateDateValidationAttribute(ErrorMessage = "The selected date can not be current or less than the current date")]
        public DateTime EndDate { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; set; }
    }

    public class EmployeeLeaveRequestViewVM
    {
        public List<LeaveAllocationVM> LeaveAllocations { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }

}
