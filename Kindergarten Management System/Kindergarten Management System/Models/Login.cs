using System.ComponentModel.DataAnnotations;

namespace Kindergarten_Management_System.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
