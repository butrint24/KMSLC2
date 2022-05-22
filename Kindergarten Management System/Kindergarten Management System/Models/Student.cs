using Kindergarten_Management_System.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kindergarten_Management_System.Models
{
    public class Student
    {
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Legal Guardian")]
        public string LegalGuardian { get; set; }

        [Required, MinLength(12, ErrorMessage = "Minimum length is 12"), MaxLength(12, ErrorMessage ="Maximum length is 12")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Legal Guardian Occupation")]
        public string GuardianOccupation { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string City { get; set; }

        public char Gender { get; set; }

        [Display(Name = "Profile picture")]
        public string Image { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose a teacher!")]
        public string TeacherName { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }


        public bool IsEmployee { get; set; }

        public DateTime Order { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }



        public Student() { }

        public Student(AppUser appUser)
        {
            FullName = appUser.FullName;
            BirthDate = appUser.BirthDate;
            LegalGuardian = appUser.LegalGuardian;
            ContactNumber = appUser.PhoneNumber;
            GuardianOccupation = appUser.GuardianOccupation;
            City = appUser.City;
            Gender = appUser.Gender;
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
            Image = appUser.StudentImage;
            IsEmployee = appUser.IsEmployee;
            Order = appUser.Order;
            TeacherName = appUser.TeacherName;
        }
    }
}
