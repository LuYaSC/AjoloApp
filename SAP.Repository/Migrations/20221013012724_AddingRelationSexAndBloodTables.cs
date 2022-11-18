using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class AddingRelationSexAndBloodTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Collaborators");

            migrationBuilder.AddColumn<int>(
                name: "BloodTypeId",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SexTypeId",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BloodTypeId",
                table: "Kids",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SexTypeId",
                table: "Kids",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BloodTypeId",
                table: "Collaborators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SexTypeId",
                table: "Collaborators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_BloodTypeId",
                table: "Parents",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_SexTypeId",
                table: "Parents",
                column: "SexTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Kids_BloodTypeId",
                table: "Kids",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Kids_SexTypeId",
                table: "Kids",
                column: "SexTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_BloodTypeId",
                table: "Collaborators",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_SexTypeId",
                table: "Collaborators",
                column: "SexTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_BloodTypes_BloodTypeId",
                table: "Collaborators",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_SexTypes_SexTypeId",
                table: "Collaborators",
                column: "SexTypeId",
                principalTable: "SexTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kids_BloodTypes_BloodTypeId",
                table: "Kids",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kids_SexTypes_SexTypeId",
                table: "Kids",
                column: "SexTypeId",
                principalTable: "SexTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_BloodTypes_BloodTypeId",
                table: "Parents",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_SexTypes_SexTypeId",
                table: "Parents",
                column: "SexTypeId",
                principalTable: "SexTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_BloodTypes_BloodTypeId",
                table: "Collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_SexTypes_SexTypeId",
                table: "Collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_Kids_BloodTypes_BloodTypeId",
                table: "Kids");

            migrationBuilder.DropForeignKey(
                name: "FK_Kids_SexTypes_SexTypeId",
                table: "Kids");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_BloodTypes_BloodTypeId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_SexTypes_SexTypeId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Parents_BloodTypeId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Parents_SexTypeId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Kids_BloodTypeId",
                table: "Kids");

            migrationBuilder.DropIndex(
                name: "IX_Kids_SexTypeId",
                table: "Kids");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_BloodTypeId",
                table: "Collaborators");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_SexTypeId",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "BloodTypeId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "SexTypeId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "BloodTypeId",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "SexTypeId",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "BloodTypeId",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "SexTypeId",
                table: "Collaborators");

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Kids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Collaborators",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
