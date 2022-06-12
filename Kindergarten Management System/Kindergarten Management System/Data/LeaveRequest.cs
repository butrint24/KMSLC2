using Kindergarten_Management_System.Models;
using Kindergarten_Management_System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Data
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RequestingEmployeeId")]

        public AppUser RequestingEmployee { get; set; }

        public string RequestingEmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey("LeaveTypeId")]

        public LeaveType LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime DateActioned { get; set; }

        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        [ForeignKey("ApprovedById")]

        public AppUser ApprovedBy { get; set; }

        public string ApprovedById { get; set; }

    }

}