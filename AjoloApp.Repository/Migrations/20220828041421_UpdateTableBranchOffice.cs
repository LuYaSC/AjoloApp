using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class UpdateTableBranchOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchOffices_Cities_CityId",
                table: "BranchOffices");

            migrationBuilder.DropIndex(
                name: "IX_BranchOffices_CityId",
                table: "BranchOffices");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "BranchOffices");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "UserDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_CityId",
                table: "UserDetails",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Cities_CityId",
                table: "UserDetails",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Cities_CityId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_CityId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "BranchOffices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BranchOffices_CityId",
                table: "BranchOffices",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchOffices_Cities_CityId",
                table: "BranchOffices",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
