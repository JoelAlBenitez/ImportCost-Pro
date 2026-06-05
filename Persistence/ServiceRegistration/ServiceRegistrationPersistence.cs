using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;
using Persistence.Repositories.ImportationOrderAndLandCost;

namespace Persistence.ServiceRegistration
{
    public static class ServiceRegistrationPersistence
    {
        public static IServiceCollection AddPersistenceRegistration(this 
            IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContextImportCost>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ImportersRepository>();
            services.AddScoped<ProductsRepository>();
            services.AddScoped<SuppliersRepository>();
            services.AddScoped<TariffCategoriesRepository>();

            // Repositorios de Importación y LandCost (Necesarios para el blindaje de Monedas y Tasas)
            services.AddScoped<ImportationOrderRepository>();
            services.AddScoped<ImportationExpenseRepository>();

            //agreguen aqui sus repositorios por favor -> 
            services.AddScoped<CountryRepository>();
            services.AddScoped<CurrencyRepository>();
            services.AddScoped<ExchangeRateRepository>();
            services.AddScoped<TaxConfigurationRepository>();

            services.AddScoped<CurrencyRepository>();
            services.AddScoped<ExchangeRateRepository>();
            services.AddScoped<TaxConfigurationRepository>();

            services.AddScoped<ImportationOrderRepository>();
            services.AddScoped<ImportationExpenseRepository>();
            services.AddScoped<OrderDetailRepository>();

            return services;
        }
    }
}
