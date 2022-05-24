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
    public class StudentEdit
    {
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [StudentDateValidationAttribute(ErrorMessage = "Ky student e ka kaluar moshen e lejuar per regjistrim ne qerdhe")]
        [Required]
        [DateValidation(ErrorMessage = "Ju lutem shenoni daten e sakt, nuk mund te jete me e madhe se data momentale")]
        [StudentMonthValidation(ErrorMessage = " Nuk lejohet me i ri se 5 muaj")]
        public DateTime BirthDate { get; set; }

        public string LegalGuardian { get; set; }

        [Required, MinLength(12, ErrorMessage = "Minimum length is 12"), MaxLength(12, ErrorMessage = "Maximum length is 12")]
        public string ContactNumber { get; set; }


        public string GuardianOccupation { get; set; }

        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose a City!")]
        public string City { get; set; }

        public char Gender { get; set; }

        public string Image { get; set; }

        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose a teacher!")]
        public string TeacherName { get; set; }

        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }



        public StudentEdit() { }

        public StudentEdit(AppUser appUser)
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
            TeacherName = appUser.TeacherName;
        }
    }
}