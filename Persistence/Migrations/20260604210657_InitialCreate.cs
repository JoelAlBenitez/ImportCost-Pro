using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsLocalCurrency = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "TariffCategories",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PorcentageTariff = table.Column<decimal>(type: "decimal(18,2)", maxLength: 100, nullable: false),
                    ITBIS = table.Column<bool>(type: "bit", nullable: false),
                    SelectiveTaxApplies = table.Column<bool>(type: "bit", nullable: false),
                    PorcentageTaxSelective = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffCategories", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "TaxConfigurations",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralItbisPercentage = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CustomsServiceRatePercentage = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxConfigurations", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Importers",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identification = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    countryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Importers", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Importers_Countries_countryId",
                        column: x => x.countryId,
                        principalTable: "Countries",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceCurrencyId = table.Column<int>(type: "int", nullable: false),
                    DestinationCurrencyId = table.Column<int>(type: "int", nullable: false),
                    RateValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_DestinationCurrencyId",
                        column: x => x.DestinationCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_SourceCurrencyId",
                        column: x => x.SourceCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MainCurrencyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Suppliers_Countries_countryId",
                        column: x => x.countryId,
                        principalTable: "Countries",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suppliers_Currencies_MainCurrencyId",
                        column: x => x.MainCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeRefence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Large = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Broad = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    High = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    tarrifCategoriesId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    countryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Products_Countries_countryId",
                        column: x => x.countryId,
                        principalTable: "Countries",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_TariffCategories_tarrifCategoriesId",
                        column: x => x.tarrifCategoriesId,
                        principalTable: "TariffCategories",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportationOrders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ImporterId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    OriginCountryId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransportMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportationOrders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_ImportationOrders_Countries_OriginCountryId",
                        column: x => x.OriginCountryId,
                        principalTable: "Countries",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportationOrders_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                   
                    table.ForeignKey(
                        name: "FK_ImportationOrders_Importers_ImporterId",
                        column: x => x.ImporterId,
                        principalTable: "Importers",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportationOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportationExpenses",
                columns: table => new
                {
                    ImportationExpenseId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImportationOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpenseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    DistributionMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImportationExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportationExpenses", x => x.ImportationExpenseId);
                    table.CheckConstraint("CK_ImportationExpenses_ExpenseAmount", "ExpenseAmount > 0");
                    table.ForeignKey(
                        name: "FK_ImportationExpenses_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportationExpenses_ImportationOrders_ImportationOrderId",
                        column: x => x.ImportationOrderId,
                        principalTable: "ImportationOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportationOrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOBUnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpectedProfitMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportationOrderDetails", x => x.OrderDetailId);
                    table.CheckConstraint("CK_ImportationOrderDetails_ExpectedProfitMargin", "ExpectedProfitMargin >= 0 and ExpectedProfitMargin < 100");
                    table.CheckConstraint("CK_ImportationOrderDetails_FOBUnitPrice", "FOBUnitPrice > 0");
                    table.CheckConstraint("CK_ImportationOrderDetails_Quantity", "Quantity > 0");
                    table.ForeignKey(
                        name: "FK_ImportationOrderDetails_ImportationOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ImportationOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportationOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandedCostSummaries",
                columns: table => new
                {
                    LandedCostSummaryId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LocalCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OriginalTotalFob = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalTotalFob = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFreight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCif = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTariff = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSelectiveTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCustomsServiceFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItbis = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLocalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalImportationCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalImportedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandedCostSummaries", x => x.LandedCostSummaryId);
                    table.CheckConstraint("CK_LandedCostSummaries_OriginalTotalFob", "OriginalTotalFob > 0");
                    table.CheckConstraint("CK_LandedCostSummaries_TotalImportationCost", "TotalImportationCost > 0");
                    table.ForeignKey(
                        name: "FK_LandedCostSummaries_ImportationOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ImportationOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandedCostDetails",
                columns: table => new
                {
                    LandedCostDetailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LandedCostSummaryId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalFOB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalFob = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssignedFreight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssignedInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cif = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tariff = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SelectiveTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomsServiceFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Itbis = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssignedLocalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalImportedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitImportedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DesiredMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuggestedSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandedCostDetails", x => x.LandedCostDetailId);
                    table.CheckConstraint("CK_LandedCostDetails_Quantity", "Quantity > 0");
                    table.CheckConstraint("CK_LandedCostDetails_UnitImportedCost", "UnitImportedCost > 0");
                    table.ForeignKey(
                        name: "FK_LandedCostDetails_LandedCostSummaries_LandedCostSummaryId",
                        column: x => x.LandedCostSummaryId,
                        principalTable: "LandedCostSummaries",
                        principalColumn: "LandedCostSummaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandedCostDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoCode",
                table: "Countries",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_IsoCode",
                table: "Currencies",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_DestinationCurrencyId",
                table: "ExchangeRates",
                column: "DestinationCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_SourceCurrencyId_DestinationCurrencyId_EffectiveDate",
                table: "ExchangeRates",
                columns: new[] { "SourceCurrencyId", "DestinationCurrencyId", "EffectiveDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportationExpenses_CurrencyId",
                table: "ImportationExpenses",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationExpenses_ImportationOrderId",
                table: "ImportationExpenses",
                column: "ImportationOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrderDetails_OrderId",
                table: "ImportationOrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrderDetails_ProductId",
                table: "ImportationOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrders_CurrencyId",
                table: "ImportationOrders",
                column: "CurrencyId");

         

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrders_ImporterId",
                table: "ImportationOrders",
                column: "ImporterId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrders_OriginCountryId",
                table: "ImportationOrders",
                column: "OriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportationOrders_SupplierId",
                table: "ImportationOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Importers_countryId",
                table: "Importers",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_LandedCostDetails_LandedCostSummaryId",
                table: "LandedCostDetails",
                column: "LandedCostSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_LandedCostDetails_ProductId",
                table: "LandedCostDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LandedCostSummaries_OrderId",
                table: "LandedCostSummaries",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_countryId",
                table: "Products",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_tarrifCategoriesId",
                table: "Products",
                column: "tarrifCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_countryId",
                table: "Suppliers",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MainCurrencyId",
                table: "Suppliers",
                column: "MainCurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "ImportationExpenses");

            migrationBuilder.DropTable(
                name: "ImportationOrderDetails");

            migrationBuilder.DropTable(
                name: "LandedCostDetails");

            migrationBuilder.DropTable(
                name: "TaxConfigurations");

            migrationBuilder.DropTable(
                name: "LandedCostSummaries");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ImportationOrders");

            migrationBuilder.DropTable(
                name: "TariffCategories");

            migrationBuilder.DropTable(
                name: "Importers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
