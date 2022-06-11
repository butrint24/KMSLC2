using Microsoft.AspNetCore.Identity;
using System;

namespace Kindergarten_Management_System.Models
{
    public class AppUserVM : IdentityUser
    {
        //Student
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string LegalGuardian { get; set; }
        public string GuardianOccupation { get; set; }
        public char Gender { get; set; }
        public string StudentImage { get; set; }
        public string City { get; set; }
        public DateTime Order { get; set; }
        public string TeacherName { get; set; }

        //Teacher Side
        public string PersonalNumber { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string TeacherImage { get; set; }

        public bool IsEmployee { get; set; }
    }
}
