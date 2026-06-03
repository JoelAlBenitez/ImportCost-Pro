using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ExchangeRates;
using Application.Services.Importers;
using Application.Services.ProductsServices;
using Application.Services.SuppliersServices;
using Application.Services.TarriffCategories;
using Application.Services.TaxConfigurations;
using Microsoft.Extensions.DependencyInjection;


namespace Application.ServicesRegistration
{
    public static class ServicesRegistrationApplication
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            
            services.AddScoped<ProductsServices>();
            services.AddScoped<TarriffCategoriesServices>();
            services.AddScoped<SuppliersServices>();
            services.AddScoped<ImportersServices>();


            

            services.AddScoped<CountryService>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<ExchangeRateService>();
            services.AddScoped<TaxConfigurationService>();

          

            return services;
        }
    }
}