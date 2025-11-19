using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeManagedByIdNullableReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Receptionists_ManagedById",
                table: "Reservations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagedById",
                table: "Reservations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Receptionists_ManagedById",
                table: "Reservations",
                column: "ManagedById",
                principalTable: "Receptionists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Receptionists_ManagedById",
                table: "Reservations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagedById",
                table: "Reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Receptionists_ManagedById",
                table: "Reservations",
                column: "ManagedById",
                principalTable: "Receptionists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
