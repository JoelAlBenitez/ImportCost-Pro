using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CambiosModeloLandedCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportationExpenses_ImportationOrders_OrderId",
                table: "ImportationExpenses");

            migrationBuilder.RenameColumn(
                name: "LocalCurrencyUsed",
                table: "LandedCostSummaries",
                newName: "LocalCurrencyId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ImportationExpenses",
                newName: "ImportationOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ImportationExpenses_OrderId",
                table: "ImportationExpenses",
                newName: "IX_ImportationExpenses_ImportationOrderId");

            migrationBuilder.AlterColumn<string>(
                name: "TariffCode",
                table: "TariffCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpenseDate",
                table: "ImportationExpenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ImportationExpenses_ImportationOrders_ImportationOrderId",
                table: "ImportationExpenses",
                column: "ImportationOrderId",
                principalTable: "ImportationOrders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportationExpenses_ImportationOrders_ImportationOrderId",
                table: "ImportationExpenses");

            migrationBuilder.DropColumn(
                name: "ExpenseDate",
                table: "ImportationExpenses");

            migrationBuilder.RenameColumn(
                name: "LocalCurrencyId",
                table: "LandedCostSummaries",
                newName: "LocalCurrencyUsed");

            migrationBuilder.RenameColumn(
                name: "ImportationOrderId",
                table: "ImportationExpenses",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ImportationExpenses_ImportationOrderId",
                table: "ImportationExpenses",
                newName: "IX_ImportationExpenses_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "TariffCode",
                table: "TariffCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportationExpenses_ImportationOrders_OrderId",
                table: "ImportationExpenses",
                column: "OrderId",
                principalTable: "ImportationOrders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
