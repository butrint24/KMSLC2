using System;
using System.ComponentModel.DataAnnotations;

namespace Kindergarten_Management_System.Models
{
    public class Contact
    {
        public Guid ContactId { get; set; }
        
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime Order { get; set; }
    }
}
