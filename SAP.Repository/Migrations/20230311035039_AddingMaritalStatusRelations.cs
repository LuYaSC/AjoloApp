using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class AddingMaritalStatusRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaritalStatusId",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_MaritalStatusId",
                table: "Parents",
                column: "MaritalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_MaritalStatus_MaritalStatusId",
                table: "Parents",
                column: "MaritalStatusId",
                principalTable: "MaritalStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_MaritalStatus_MaritalStatusId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Parents_MaritalStatusId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MaritalStatusId",
                table: "Parents");
        }
    }
}
