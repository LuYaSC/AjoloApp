using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class addBranchOffices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchOfficeId",
                table: "AssignedRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchOfficeId",
                table: "AspNetUserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cities_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchOffices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchOffices_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchOffices_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchOffices_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_BranchOfficeId",
                table: "AspNetUserRoles",
                column: "BranchOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchOffices_CityId",
                table: "BranchOffices",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchOffices_UserCreation",
                table: "BranchOffices",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_BranchOffices_UserModification",
                table: "BranchOffices",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UserCreation",
                table: "Cities",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UserModification",
                table: "Cities",
                column: "UserModification");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_BranchOffices_BranchOfficeId",
                table: "AspNetUserRoles",
                column: "BranchOfficeId",
                principalTable: "BranchOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_BranchOffices_BranchOfficeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "BranchOffices");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_BranchOfficeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "BranchOfficeId",
                table: "AssignedRooms");

            migrationBuilder.DropColumn(
                name: "BranchOfficeId",
                table: "AspNetUserRoles");
        }
    }
}
