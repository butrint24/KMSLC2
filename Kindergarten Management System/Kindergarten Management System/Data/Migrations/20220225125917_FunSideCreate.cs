using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class FunSideCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FunSides",
                columns: table => new
                {
                    FunSideId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Order = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunSides", x => x.FunSideId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunSides");
        }
    }
}
