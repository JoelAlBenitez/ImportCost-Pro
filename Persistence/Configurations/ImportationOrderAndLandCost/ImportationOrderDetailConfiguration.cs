using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.ImportationOrderAndLandCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.ImportationOrderAndLandCost
{
    public class ImportationOrderDetailConfiguration : IEntityTypeConfiguration<ImportationOrderDetail>
    {
        public void Configure(EntityTypeBuilder<ImportationOrderDetail> builder)
        {
            builder.HasKey(x => x.OrderDetailId);
            builder.ToTable("ImportationOrderDetails", t =>
            {
                t.HasCheckConstraint("CK_ImportationOrderDetails_Quantity", "Quantity > 0");
                t.HasCheckConstraint("CK_ImportationOrderDetails_FOBUnitPrice", "FOBUnitPrice > 0");
                t.HasCheckConstraint("CK_ImportationOrderDetails_ExpectedProfitMargin", "ExpectedProfitMargin > 0 and ExpectedProfitMargin < 100");
            });

            #region Property Configurations

            builder.HasOne(x => x.ImportationOrder).WithMany(y => y.ImportationOrderDetails).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.FOBUnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.ExpectedProfitMargin).IsRequired().HasColumnType("decimal(18,2)");

            #endregion
        }
    }
}
