using Kindergarten_Management_System.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kindergarten_Management_System.Models
{
    public class Application
    {
        public Guid ApplicationId { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [StudentDateValidationAttribute(ErrorMessage = "Ky student e ka kaluar moshen e lejuar per regjistrim ne qerdhe")]
        [DateValidation(ErrorMessage = "Ju lutem shenoni daten e sakt, nuk mund te jete me e madhe se data momentale")]
        [StudentMonthValidation(ErrorMessage = " Nuk lejohet me i ri se 5 muaj")]
        public DateTime BirthDate { get; set; }

        [Required, MinLength(10, ErrorMessage = "Minimum length is 10"), MaxLength(10, ErrorMessage = "Maximum length is 10")]
        public string PersonalNumber { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string LegalGuardian { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string LegalGuardianOcupation { get; set; }

        [Required, MinLength(12, ErrorMessage = "Minimum length is 12"), MaxLength(12, ErrorMessage = "Maximum length is 12")]
        public string ContactNumber { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose a city!")]
        public string City { get; set; }
        public DateTime Order { get; set; }
        public char Gender { get; set; }
        public string Image { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Please choose an offer!")]
        public string Offer { get; set; }


        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}