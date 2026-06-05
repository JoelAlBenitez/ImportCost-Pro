using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.ImportationOrderAndLandCost;


namespace Persistence.Configurations.ImportationOrderAndLandCost
{
    public class ImportationOrderConfiguration : IEntityTypeConfiguration<ImportationOrder>
    {
        public void Configure(EntityTypeBuilder<ImportationOrder> builder)
        {
            builder.HasKey(orID => orID.OrderId);
            builder.Property(x => x.OrderId).HasMaxLength(30);
            builder.ToTable("ImportationOrders");

            #region Property Configurations
            builder.HasOne(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.OriginCountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Cascade);
            
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TransportMode).IsRequired().HasConversion<string>();
            builder.Property(x => x.OrderState).IsRequired().HasConversion<string>();

            //arreglo de relación uno a uno entre ImportationOrder y LandedCostSummary
            builder.HasOne(o => o.LandedCostSummary)
             .WithOne(s => s.ImportationOrder) 
             .HasForeignKey<LandedCostSummary>(s => s.OrderId)
             .OnDelete(DeleteBehavior.Cascade); 

            #endregion
        }
    }
}