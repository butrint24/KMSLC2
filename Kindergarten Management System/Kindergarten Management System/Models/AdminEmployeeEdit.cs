using Kindergarten_Management_System.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kindergarten_Management_System.Models
{
    public class AdminEmployeeEdit
    {
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [EmployeeDateValidationAttribute(ErrorMessage = "Ju mund te regjistroheni vetem ne listen e penzionereve")]
        [DateValidation(ErrorMessage = "Ju lutem shenoni daten e sakt, nuk mund te jete me e madhe se data momentale")]
        [EmployeeYearValidation(ErrorMessage = "Nuk mund te regjistrohet nje employee me i ri se 18vjet")]
        public DateTime BirthDate { get; set; }

        [Required, MinLength(10, ErrorMessage = "Minimum length is 4"), MaxLength(12, ErrorMessage = "Maximum length is 10")]
        public string PersonalNumber { get; set; }

        [Required, MinLength(12, ErrorMessage = "Minimum length is 12"), MaxLength(12, ErrorMessage = "Maximum length is 12")]
        public string ContactNumber { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Title { get; set; }

        [Required, MinLength(15, ErrorMessage = "Minimum length is 15")]
        public string Bio { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose a city!")]
        public string City { get; set; }

        public char Gender { get; set; }

        public string Image { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }


        public bool IsEmployee { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }



        public AdminEmployeeEdit() { }

        public AdminEmployeeEdit(AppUser appUser)
        {
            FullName = appUser.FullName;
            BirthDate = appUser.BirthDate;
            PersonalNumber = appUser.PersonalNumber;
            Title = appUser.Title;
            Bio = appUser.Bio;
            City = appUser.City;
            Gender = appUser.Gender;
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
            Image = appUser.StudentImage;
            IsEmployee = appUser.IsEmployee;
            ContactNumber = appUser.PhoneNumber;
        }
    }
}

