using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCurrencyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyKey1",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyKey1",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyKey",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyKey1",
                table: "ExchangeRates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
