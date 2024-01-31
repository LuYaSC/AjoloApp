using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class AddingTablesforBloodAndSexTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Initial = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodTypes_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodTypes_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SexTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Initial = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SexTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SexTypes_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SexTypes_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodTypes_UserCreation",
                table: "BloodTypes",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_BloodTypes_UserModification",
                table: "BloodTypes",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_SexTypes_UserCreation",
                table: "SexTypes",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_SexTypes_UserModification",
                table: "SexTypes",
                column: "UserModification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodTypes");

            migrationBuilder.DropTable(
                name: "SexTypes");
        }
    }
}
