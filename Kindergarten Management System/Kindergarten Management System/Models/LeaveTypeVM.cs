using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Range(1, 31, ErrorMessage = "please enter a valid number")]
        public int DefaultDays { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }
    }
}
