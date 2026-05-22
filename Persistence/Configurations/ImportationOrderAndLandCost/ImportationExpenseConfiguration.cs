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
    public class ImportationExpenseConfiguration : IEntityTypeConfiguration<ImportationExpense>
    {
        public void Configure(EntityTypeBuilder<ImportationExpense> builder)
        {
            builder.HasKey(x => x.ImportationExpenseId);


            builder.ToTable("ImportationExpenses", t =>
            {
                t.HasCheckConstraint("ImportationExpenseId_ExpenseAmount", "ExpenseAmount > 0");
            });

            #region Property Configurations

            builder.HasOne(x => x.ImportationOrder).WithMany(y => y.ImportationExpenses).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.ExpenseType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ExpenseAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CurrencyId).IsRequired().HasMaxLength(3);
            builder.Property(x => x.DistributionMethod).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ImportationExpenseDate).IsRequired();


            #endregion
        }
    }
}
