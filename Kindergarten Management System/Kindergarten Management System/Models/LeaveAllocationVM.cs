using Kindergarten_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Models
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        public int NumberOfDays { get; set; }

        public DateTime DateCreated { get; set; }

        public int Period { get; set; }

        public AppUserVM AppUser { get; set; }

        public string employeeId { get; set; }

        public LeaveTypeVM LeaveType { get; set; }

        public int LeaveTypeId { get; set; }
        
        public IEnumerable<SelectListItem> AppUserVM { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }

    public class CreateLeaveAllocationVM
    {
        public int NumberUpdated { get; set; }

        public List<LeaveTypeVM> LeaveType { get; set; }
    }

    public class ViewAllocationsVM
    {
        public AppUserVM Employee { get; set; }

        public string EmployeeId { get; set; }

        public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    }

}
