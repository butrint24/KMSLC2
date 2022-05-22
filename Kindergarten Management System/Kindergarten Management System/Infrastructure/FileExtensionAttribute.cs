using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extenstion = Path.GetExtension(file.FileName);

                string[] extensions = { "png", "jpg" };
                bool result = extensions.Any(x => extenstion.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }


            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Allowed picture formats png, jpg!";

        }
    }
}