using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class homeworkSubCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks");

            migrationBuilder.CreateTable(
                name: "HomeWorkSubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    StudentName = table.Column<string>(nullable: true),
                    TeacherId = table.Column<string>(nullable: true),
                    HomeworkId = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Order = table.Column<DateTime>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWorkSubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubs_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubs_AppUserId",
                table: "HomeWorkSubs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks");

            migrationBuilder.DropTable(
                name: "HomeWorkSubs");

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
