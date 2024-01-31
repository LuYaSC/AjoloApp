using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class CreatePaymentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditPaymentId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumberBill",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentOperationId",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Initial = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPaymentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPaymentTypes_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPaymentTypes_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Initial = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOperations_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentOperations_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AuditPaymentId",
                table: "Payments",
                column: "AuditPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentOperationId",
                table: "Payments",
                column: "PaymentOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPaymentTypes_UserCreation",
                table: "AuditPaymentTypes",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPaymentTypes_UserModification",
                table: "AuditPaymentTypes",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOperations_UserCreation",
                table: "PaymentOperations",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOperations_UserModification",
                table: "PaymentOperations",
                column: "UserModification");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AuditPaymentTypes_AuditPaymentId",
                table: "Payments",
                column: "AuditPaymentId",
                principalTable: "AuditPaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentOperations_PaymentOperationId",
                table: "Payments",
                column: "PaymentOperationId",
                principalTable: "PaymentOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AuditPaymentTypes_AuditPaymentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentOperations_PaymentOperationId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "AuditPaymentTypes");

            migrationBuilder.DropTable(
                name: "PaymentOperations");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AuditPaymentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentOperationId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AuditPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "NumberBill",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentOperationId",
                table: "Payments");
        }
    }
}
