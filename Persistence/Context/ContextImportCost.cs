using Microsoft.EntityFrameworkCore;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Entities.OperationalCommercial;
using System.Reflection;

namespace Persistence.Context
{
    public class ContextImportCost : DbContext
    {
        public ContextImportCost(DbContextOptions<ContextImportCost> op) : base (op) { }

        public DbSet<Importers> importers { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Suppliers> suppliers { get; set; }
        public DbSet<TariffCategories> tariffCategories { get; set; }


        //Importation Order and Landed Cost sets

        public DbSet<ImportationOrder> ImportationOrders { get; set; }
        public DbSet<ImportationOrderDetail> ImportationOrderDetails { get; set; }
        public DbSet<ImportationExpense> ImportationExpenses { get; set; }
        public DbSet<LandedCostDetail> LandedCostDetails { get; set; }
        public DbSet<LandedCostSummary> LandedCostSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

    }
}
