using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class DeleteFieldsKidNotUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgePregnancyMother",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "IsPlanified",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "Medicaments",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "MotherDrink",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "PhysicalsConditions",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "PrenatalCheckup",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "PsychologicalConditions",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "ThreatenedAbortion",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "UrineTest",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "XRaysThirdMonth",
                table: "Kids");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgePregnancyMother",
                table: "Kids",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlanified",
                table: "Kids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Medicaments",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotherDrink",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalsConditions",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PrenatalCheckup",
                table: "Kids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PsychologicalConditions",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ThreatenedAbortion",
                table: "Kids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UrineTest",
                table: "Kids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "XRaysThirdMonth",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
