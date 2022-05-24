using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class HomeworkCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.CreateTable(
                name: "HomeWorks",
                columns: table => new
                {
                    HomeWorkId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: false),
                    Order = table.Column<DateTime>(nullable: false),
                    TeacherId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWorks", x => x.HomeWorkId);
                    table.ForeignKey(
                        name: "FK_HomeWorks_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorks_TeacherId",
                table: "HomeWorks",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropTable(
                name: "HomeWorks");

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
