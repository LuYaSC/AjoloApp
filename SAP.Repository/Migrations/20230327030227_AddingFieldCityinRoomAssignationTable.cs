using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class AddingFieldCityinRoomAssignationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AssignedRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignedRooms_CityId",
                table: "AssignedRooms",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedRooms_Cities_CityId",
                table: "AssignedRooms",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedRooms_Cities_CityId",
                table: "AssignedRooms");

            migrationBuilder.DropIndex(
                name: "IX_AssignedRooms_CityId",
                table: "AssignedRooms");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AssignedRooms");
        }
    }
}
