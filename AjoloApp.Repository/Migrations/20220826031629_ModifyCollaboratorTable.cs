using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class ModifyCollaboratorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Collaborators",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Collaborators");
        }
    }
}
