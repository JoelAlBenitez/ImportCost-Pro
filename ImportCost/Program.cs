using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;
using Persistence.Repositories.ImportationOrderAndLandCost;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ExchangeRates;
using Application.Services.TaxConfigurations;
using Application.Services.Importers;
using Application.Services.ProductsServices;
using Application.Services.SuppliersServices;
using Application.Services.TarriffCategories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ContextImportCost>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories Registration
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<CurrencyRepository>();
builder.Services.AddScoped<ExchangeRateRepository>();
builder.Services.AddScoped<TaxConfigurationRepository>();
builder.Services.AddScoped<ImportersRepository>();
builder.Services.AddScoped<ProductsRepository>();
builder.Services.AddScoped<SuppliersRepository>();
builder.Services.AddScoped<TariffCategoriesRepository>();
builder.Services.AddScoped<ImportationOrderRepository>();
builder.Services.AddScoped<ImportationExpenseRepository>();

// Services Registration
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<ExchangeRateService>();
builder.Services.AddScoped<TaxConfigurationService>();
builder.Services.AddScoped<ImportersServices>();
builder.Services.AddScoped<ProductsServices>();
builder.Services.AddScoped<SuppliersServices>();
builder.Services.AddScoped<TarriffCategoriesServices>();

builder.Environment.IsDevelopment();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
