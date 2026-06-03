using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;

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

            //agreguen aqui sus repositorios por favor -> 
            services.AddScoped<CountryRepository>();
            services.AddScoped<CurrencyRepository>();
            services.AddScoped<ExchangeRateRepository>();
            services.AddScoped<TaxConfigurationRepository>();

            return services;
        }
    }
}
