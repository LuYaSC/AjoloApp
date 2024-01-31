using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class ModifyKidEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kids_KidBackgrounds_KidBackgroudId",
                table: "Kids");

            migrationBuilder.DropTable(
                name: "KidBackgrounds");

            migrationBuilder.DropIndex(
                name: "IX_Kids_KidBackgroudId",
                table: "Kids");

            migrationBuilder.RenameColumn(
                name: "KidBackgroudId",
                table: "Kids",
                newName: "AgePregnancyMother");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Kids",
                newName: "UrineTest");

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

            migrationBuilder.AddColumn<string>(
                name: "XRaysThirdMonth",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "XRaysThirdMonth",
                table: "Kids");

            migrationBuilder.RenameColumn(
                name: "UrineTest",
                table: "Kids",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "AgePregnancyMother",
                table: "Kids",
                newName: "KidBackgroudId");

            migrationBuilder.CreateTable(
                name: "KidBackgrounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    AgePregnancyMother = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsPlanified = table.Column<bool>(type: "boolean", nullable: false),
                    Medicaments = table.Column<string>(type: "text", nullable: false),
                    MotherDrink = table.Column<string>(type: "text", nullable: false),
                    PhysicalsConditions = table.Column<string>(type: "text", nullable: false),
                    PrenatalCheckup = table.Column<bool>(type: "boolean", nullable: false),
                    PsychologicalConditions = table.Column<string>(type: "text", nullable: false),
                    ThreatenedAbortion = table.Column<bool>(type: "boolean", nullable: false),
                    UrineTest = table.Column<bool>(type: "boolean", nullable: false),
                    XRaysThirdMonth = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidBackgrounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidBackgrounds_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidBackgrounds_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kids_KidBackgroudId",
                table: "Kids",
                column: "KidBackgroudId");

            migrationBuilder.CreateIndex(
                name: "IX_KidBackgrounds_UserCreation",
                table: "KidBackgrounds",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidBackgrounds_UserModification",
                table: "KidBackgrounds",
                column: "UserModification");

            migrationBuilder.AddForeignKey(
                name: "FK_Kids_KidBackgrounds_KidBackgroudId",
                table: "Kids",
                column: "KidBackgroudId",
                principalTable: "KidBackgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
