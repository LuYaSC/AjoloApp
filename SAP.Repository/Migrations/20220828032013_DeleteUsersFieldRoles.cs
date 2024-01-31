using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class DeleteUsersFieldRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCreation",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UserModification",
                table: "AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCreation",
                table: "AspNetRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserModification",
                table: "AspNetRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
