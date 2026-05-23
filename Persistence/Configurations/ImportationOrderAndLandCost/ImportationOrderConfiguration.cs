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
            builder.ToTable("ImportationOrders");

            #region Property Configurations
            builder.HasOne(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.OriginCountryId).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired().HasMaxLength(3);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TransportMode).IsRequired().HasConversion<string>();
            builder.Property(x => x.OrderState).IsRequired().HasConversion<string>();

            #endregion
        }
    }
}