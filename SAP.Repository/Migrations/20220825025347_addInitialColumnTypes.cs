using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class addInitialColumnTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "Turns",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "Relationship",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "PaymentTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "Modalities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "DocumentTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "Cities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initial",
                table: "BranchOffices",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Initial",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "Relationship");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "Modalities");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Initial",
                table: "BranchOffices");
        }
    }
}
