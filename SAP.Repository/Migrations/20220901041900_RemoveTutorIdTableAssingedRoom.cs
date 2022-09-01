using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class RemoveTutorIdTableAssingedRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedRooms_AssignedTutors_AssignedTutorId",
                table: "AssignedRooms");

            migrationBuilder.DropIndex(
                name: "IX_AssignedRooms_AssignedTutorId",
                table: "AssignedRooms");

            migrationBuilder.DropColumn(
                name: "AssignedTutorId",
                table: "AssignedRooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedTutorId",
                table: "AssignedRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignedRooms_AssignedTutorId",
                table: "AssignedRooms",
                column: "AssignedTutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedRooms_AssignedTutors_AssignedTutorId",
                table: "AssignedRooms",
                column: "AssignedTutorId",
                principalTable: "AssignedTutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
