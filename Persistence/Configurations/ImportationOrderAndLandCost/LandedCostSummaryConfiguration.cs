using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.ImportationOrderAndLandCost;


namespace Persistence.Configurations.ImportationOrderAndLandCost
{
    public class LandedCostSummaryConfiguration : IEntityTypeConfiguration<LandedCostSummary>
    {
        public void Configure(EntityTypeBuilder<LandedCostSummary> builder)
        {
            builder.HasKey(x => x.LandedCostSummaryId);
            builder.Property(x => x.LandedCostSummaryId).HasMaxLength(50);
            builder.Property(x => x.OrderId).HasMaxLength(30);

            builder.ToTable("LandedCostSummaries", t =>
            {
                t.HasCheckConstraint("CK_LandedCostSummaries_TotalImportationCost", "TotalImportationCost > 0");
                t.HasCheckConstraint("CK_LandedCostSummaries_OriginalTotalFob", "OriginalTotalFob > 0");
            });

            #region Property Configurations
            builder.HasOne(x=> x.ImportationOrder).WithOne(y  => y.LandedCostSummary).HasForeignKey<LandedCostSummary>(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.LocalCurrencyId).IsRequired();
            builder.Property(x => x.ExchangeRate).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.OriginalTotalFob).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.LocalTotalFob).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalFreight).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalInsurance).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalCif).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalTariff).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalSelectiveTax).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalCustomsServiceFee).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalItbis).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalLocalExpenses).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalImportationCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalImportedQuantity).IsRequired().HasColumnType("decimal(18,2)");
            #endregion
        }
    }
}
