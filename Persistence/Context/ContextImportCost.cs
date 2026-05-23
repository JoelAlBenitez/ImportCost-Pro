using Microsoft.EntityFrameworkCore;
using Persistence.Entities.FinancialCore;
using Persistence.Entities.OperationalCommercial;
using System.Reflection;

namespace Persistence.Context
{
    public class ContextImportCost : DbContext
    {
        public ContextImportCost(DbContextOptions<ContextImportCost> op) : base (op) { }

        public DbSet<Country> countries { get; set; }
        public DbSet<Importers> importers { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Suppliers> suppliers { get; set; }
        public DbSet<TariffCategories> tariffCategories { get; set; }

        public DbSet<Currency> currencies { get; set; }
        public DbSet<ExchangeRate> exchangeRates { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

    }
}
