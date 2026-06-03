using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompleteFinancialCoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyKey",
                table: "ImportationOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyKey",
                table: "ImportationExpenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyKey",
                table: "ExchangeRates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyKey1",
                table: "ExchangeRates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrders_CurrencyKey",
                table: "ImportationOrders",
                column: "CurrencyKey");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationExpenses_CurrencyKey",
                table: "ImportationExpenses",
                column: "CurrencyKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyKey",
                table: "ExchangeRates",
                column: "CurrencyKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyKey1",
                table: "ExchangeRates",
                column: "CurrencyKey1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey",
                table: "ExchangeRates",
                column: "CurrencyKey",
                principalTable: "Currencies",
                principalColumn: "Key");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey1",
                table: "ExchangeRates",
                column: "CurrencyKey1",
                principalTable: "Currencies",
                principalColumn: "Key");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportationExpenses_Currencies_CurrencyKey",
                table: "ImportationExpenses",
                column: "CurrencyKey",
                principalTable: "Currencies",
                principalColumn: "Key");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportationOrders_Currencies_CurrencyKey",
                table: "ImportationOrders",
                column: "CurrencyKey",
                principalTable: "Currencies",
                principalColumn: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey1",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportationExpenses_Currencies_CurrencyKey",
                table: "ImportationExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportationOrders_Currencies_CurrencyKey",
                table: "ImportationOrders");

            migrationBuilder.DropIndex(
                name: "IX_ImportationOrders_CurrencyKey",
                table: "ImportationOrders");

            migrationBuilder.DropIndex(
                name: "IX_ImportationExpenses_CurrencyKey",
                table: "ImportationExpenses");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyKey1",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyKey",
                table: "ImportationOrders");

            migrationBuilder.DropColumn(
                name: "CurrencyKey",
                table: "ImportationExpenses");

            migrationBuilder.DropColumn(
                name: "CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyKey1",
                table: "ExchangeRates");
        }
    }
}
