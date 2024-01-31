using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class fieldbodyintablelogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseBody",
                table: "ManageLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseBody",
                table: "ManageLogs");
        }
    }
}
