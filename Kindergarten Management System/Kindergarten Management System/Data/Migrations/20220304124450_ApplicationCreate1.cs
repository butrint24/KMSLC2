using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class ApplicationCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    PersonalNumber = table.Column<string>(maxLength: 10, nullable: false),
                    LegalGuardian = table.Column<string>(nullable: false),
                    LegalGuardianOcupation = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(maxLength: 12, nullable: false),
                    City = table.Column<string>(nullable: false),
                    Order = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
