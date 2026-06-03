using Application.Services.Currencies;
using Application.Services.ImportationExpenseServices;
using Application.Services.ImportationOrderServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories.ImportationOrderAndLandCost;

using Application.ServicesRegistration;
using Persistence.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<ContextImportCost>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddApplicationRegistration();
builder.Services.AddPersistenceRegistration(builder.Configuration);

builder.Environment.IsDevelopment();

builder.Services.AddScoped<Persistence.Repositories.FinancialCore.CurrencyRepository>();
builder.Services.AddScoped<Persistence.Repositories.FinancialCore.ExchangeRateRepository>();
builder.Services.AddScoped<Persistence.Repositories.FinancialCore.TaxConfigurationRepository>();

builder.Services.AddScoped<Persistence.Repositories.ImportationOrderAndLandCost.ImportationOrderRepository>();
builder.Services.AddScoped<Persistence.Repositories.ImportationOrderAndLandCost.ImportationExpenseRepository>();
builder.Services.AddScoped<Persistence.Repositories.ImportationOrderAndLandCost.OrderDetailRepository>();

builder.Services.AddScoped<ImportationOrderService>();
builder.Services.AddScoped<ImportationExpenseService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<LandedCostService>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
