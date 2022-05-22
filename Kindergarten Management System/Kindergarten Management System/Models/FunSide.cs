using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kindergarten_Management_System.Models
{
    public class FunSide
    {
        public Guid FunSideId { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime Order { get; set; }

        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual AppUser AppUser { get; set; }
    }
}