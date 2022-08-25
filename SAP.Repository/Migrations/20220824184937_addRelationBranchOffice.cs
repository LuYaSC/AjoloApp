using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class addRelationBranchOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssignedRooms_BranchOfficeId",
                table: "AssignedRooms",
                column: "BranchOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedRooms_BranchOffices_BranchOfficeId",
                table: "AssignedRooms",
                column: "BranchOfficeId",
                principalTable: "BranchOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedRooms_BranchOffices_BranchOfficeId",
                table: "AssignedRooms");

            migrationBuilder.DropIndex(
                name: "IX_AssignedRooms_BranchOfficeId",
                table: "AssignedRooms");
        }
    }
}
