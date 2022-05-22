using Kindergarten_Management_System.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Models
{
    public class EmployeeEdit
    {

        public string FullName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }

        public string PersonalNumber { get; set; }

        public string City { get; set; }

        public char Gender { get; set; }

        [Required, MinLength(12, ErrorMessage = "Minimum length is 12"), MaxLength(12, ErrorMessage = "Maximum length is 12")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Title { get; set; }

        [Required, MinLength(15, ErrorMessage = "Minimum length is 15")]
        public string Bio { get; set; }


        [Display(Name = "Profile picture")]
        public string Image { get; set; }

        [MinLength(3, ErrorMessage = "Minimum length is 3")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }



        public EmployeeEdit() { }

        public EmployeeEdit(AppUser appUser)
        {
            FullName = appUser.FullName;
            BirthDate = appUser.BirthDate;
            PersonalNumber = appUser.PersonalNumber;
            City = appUser.City;
            Gender = appUser.Gender;
            Title = appUser.Title;
            Bio = appUser.Bio;
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
            Image = appUser.TeacherImage;
            ContactNumber = appUser.PhoneNumber;
        }
    }
}
