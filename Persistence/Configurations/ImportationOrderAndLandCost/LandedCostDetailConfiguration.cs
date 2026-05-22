using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.ImportationOrderAndLandCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.ImportationOrderAndLandCost
{
    public class LandedCostDetailConfiguration : IEntityTypeConfiguration<LandedCostDetail>
    {
        public void Configure(EntityTypeBuilder<LandedCostDetail> builder)
        {
            builder.HasKey(x => x.LandedCostDetailId);
            builder.ToTable("LandedCostDetails", t =>
            {
                t.HasCheckConstraint("CK_LandedCostDetails_FinalUnitCost", "UnitImportedCost > 0");
                t.HasCheckConstraint("CK_LandedCostDetails_Quantity", "Quantity > 0");
            });

            builder.HasOne(x => x.LandedCostSummary).WithMany(y=> y.LandedCostDetails).HasForeignKey(x => x.LandedCostSummaryId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.OriginalFOB).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.LocalFob).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.AssignedFreight).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.AssignedInsurance).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Cif).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Tariff).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.SelectiveTax).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CustomsServiceFee).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Itbis).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.AssignedLocalExpenses).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalImportedCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.UnitImportedCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.DesiredMargin).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.SuggestedSalePrice).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
