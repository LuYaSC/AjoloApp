using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class changesPaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "AssignedTutorId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AssignedTutorId",
                table: "Payments",
                column: "AssignedTutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AssignedTutors_AssignedTutorId",
                table: "Payments",
                column: "AssignedTutorId",
                principalTable: "AssignedTutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AssignedTutors_AssignedTutorId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AssignedTutorId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AssignedTutorId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");
        }
    }
}
