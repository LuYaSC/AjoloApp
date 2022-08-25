using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class addRelationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "AssignedTutors");

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "AssignedTutors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RelationshipId",
                table: "AssignedTutors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Relationship",
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
                    table.PrimaryKey("PK_Relationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationship_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationship_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTutors_RelationshipId",
                table: "AssignedTutors",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_UserCreation",
                table: "Relationship",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_UserModification",
                table: "Relationship",
                column: "UserModification");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTutors_Relationship_RelationshipId",
                table: "AssignedTutors",
                column: "RelationshipId",
                principalTable: "Relationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTutors_Relationship_RelationshipId",
                table: "AssignedTutors");

            migrationBuilder.DropTable(
                name: "Relationship");

            migrationBuilder.DropIndex(
                name: "IX_AssignedTutors_RelationshipId",
                table: "AssignedTutors");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "AssignedTutors");

            migrationBuilder.DropColumn(
                name: "RelationshipId",
                table: "AssignedTutors");

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "AssignedTutors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
