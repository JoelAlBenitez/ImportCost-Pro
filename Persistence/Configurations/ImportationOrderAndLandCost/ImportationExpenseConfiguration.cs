using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.ImportationOrderAndLandCost;


namespace Persistence.Configurations.ImportationOrderAndLandCost
{
    public class ImportationExpenseConfiguration : IEntityTypeConfiguration<ImportationExpense>
    {
        public void Configure(EntityTypeBuilder<ImportationExpense> builder)
        {
            builder.HasKey(x => x.ImportationExpenseId);
            builder.Property(x => x.ImportationExpenseId).HasMaxLength(50);
            builder.Property(x => x.ImportationOrderId).HasMaxLength(30);


            builder.ToTable("ImportationExpenses", t =>
            {
                t.HasCheckConstraint("CK_ImportationExpenses_ExpenseAmount", "ExpenseAmount > 0");
            });

            #region Property Configurations

            builder.HasOne(x => x.ImportationOrder).WithMany(y => y.ImportationExpenses).HasForeignKey(x => x.ImportationOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ExpenseType).IsRequired().HasConversion<string>();
            builder.Property(x => x.ExpenseAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.DistributionMethod).IsRequired().HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.ImportationExpenseDate).IsRequired();


            #endregion
        }
    }
}
