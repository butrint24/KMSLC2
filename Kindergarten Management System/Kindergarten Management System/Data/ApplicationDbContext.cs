using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kindergarten_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }
        public DbSet<Briefing> Briefings { get; set; }
        public DbSet<FunSide> FunSides { get; set; }
        public DbSet<HomeWork> HomeWorks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
