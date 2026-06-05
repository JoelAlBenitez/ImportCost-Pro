using Application.Services.Currencies;
using Application.Services.ImportationExpenseServices;
using Application.Services.ImportationOrderDetailServices;
using Application.Services.ImportationOrderServices;
using Application.ServicesRegistration;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories.ImportationOrderAndLandCost;
using Persistence.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Conexión a la base de datos
builder.Services.AddDbContext<ContextImportCost>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro unificado usando los métodos de extensión (Arquitectura limpia del equipo)
builder.Services.AddApplicationRegistration();
builder.Services.AddPersistenceRegistration(builder.Configuration);

builder.Environment.IsDevelopment();



builder.Services.AddScoped<ImportationOrderService>();
builder.Services.AddScoped<ImportationExpenseService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<LandedCostService>();
builder.Services.AddScoped<ImportationOrderDetailService>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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