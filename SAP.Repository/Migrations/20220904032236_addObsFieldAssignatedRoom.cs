using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class addObsFieldAssignatedRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "AssignedRooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observations",
                table: "AssignedRooms");
        }
    }
}
