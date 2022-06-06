using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class AppOfferUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks");

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                table: "Applications",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_AspNetUsers_TeacherId",
                table: "HomeWorks");

            migrationBuilder.DropColumn(
                name: "Offer",
                table: "Applications");

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
