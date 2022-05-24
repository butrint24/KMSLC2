using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Models
{
    public class HomeWork
    {
        public Guid HomeWorkId { get; set; }

        [Required, MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Title { get; set; }
        public string Slug { get; set; }

        [Required, MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Content { get; set; }
        public DateTime Order { get; set; }

        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual AppUser AppUser { get; set; }

        internal Task<object> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
