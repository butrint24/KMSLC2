using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kindergarten_Management_System.Models
{
    public class HomeWorkSub
    {
        [Key]
        public Guid Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string TeacherId { get; set; }
        public string HomeworkId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        
        public string FileName { get; set; }
        public DateTime Order { get; set; }

        public virtual AppUser AppUser { get; set; }

        //[ForeignKey("HomeworkId")]
        //public virtual HomeWork HomeWork { get; set; }
    }
}
