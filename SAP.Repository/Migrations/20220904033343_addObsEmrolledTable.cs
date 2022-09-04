using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class addObsEmrolledTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "EnrolledChildrens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observations",
                table: "EnrolledChildrens");
        }
    }
}
