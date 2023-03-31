using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAP.Repository.Migrations
{
    public partial class ChangesinPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AssignedRooms_AssignedRoomId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AssignedTutors_AssignedTutorId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AssignedRoomId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AssignedRoomId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "AssignedTutorId",
                table: "Payments",
                newName: "EnrolledChildrenId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_AssignedTutorId",
                table: "Payments",
                newName: "IX_Payments_EnrolledChildrenId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateToPay",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_EnrolledChildrens_EnrolledChildrenId",
                table: "Payments",
                column: "EnrolledChildrenId",
                principalTable: "EnrolledChildrens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_EnrolledChildrens_EnrolledChildrenId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DateToPay",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "EnrolledChildrenId",
                table: "Payments",
                newName: "AssignedTutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_EnrolledChildrenId",
                table: "Payments",
                newName: "IX_Payments_AssignedTutorId");

            migrationBuilder.AddColumn<int>(
                name: "AssignedRoomId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AssignedRoomId",
                table: "Payments",
                column: "AssignedRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AssignedRooms_AssignedRoomId",
                table: "Payments",
                column: "AssignedRoomId",
                principalTable: "AssignedRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AssignedTutors_AssignedTutorId",
                table: "Payments",
                column: "AssignedTutorId",
                principalTable: "AssignedTutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
