using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class UpdatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BornDate",
                table: "Collaborators",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "Collaborators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EnrolledChildrens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssignedTutorId = table.Column<int>(type: "integer", nullable: false),
                    AssignedRoomId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolledChildrens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrolledChildrens_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrolledChildrens_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrolledChildrens_AssignedRooms_AssignedRoomId",
                        column: x => x.AssignedRoomId,
                        principalTable: "AssignedRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrolledChildrens_AssignedTutors_AssignedTutorId",
                        column: x => x.AssignedTutorId,
                        principalTable: "AssignedTutors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledChildrens_AssignedRoomId",
                table: "EnrolledChildrens",
                column: "AssignedRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledChildrens_AssignedTutorId",
                table: "EnrolledChildrens",
                column: "AssignedTutorId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledChildrens_UserCreation",
                table: "EnrolledChildrens",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolledChildrens_UserModification",
                table: "EnrolledChildrens",
                column: "UserModification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrolledChildrens");

            migrationBuilder.DropColumn(
                name: "BornDate",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "Collaborators");
        }
    }
}
