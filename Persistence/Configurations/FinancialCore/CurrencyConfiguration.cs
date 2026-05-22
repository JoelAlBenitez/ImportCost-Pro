using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.Base;
using Persistence.Entities.FinancialCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.FinancialCore
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");
            builder.HasKey(c => c.Key);
            builder.Property(c => c.IsLocalCurrency).IsRequired();
            builder.Property(c => c.IsoCode).HasMaxLength(3).IsRequired();
            builder.HasIndex(c => c.IsoCode)
                .IsUnique();

            builder.Property(c => c.Name).HasMaxLength(100);
            builder.Property(c => c.Symbol).HasMaxLength(10);
            builder.Property(c => c.State).HasDefaultValue(true);
            builder.HasMany(c => c.Suppliers);
                   //.WithOne(c => c.MainCurrencys)
                   //.HasForeignKey(s => s.MainCurrencyId)
                   //.OnDelete(DeleteBehavior.Restrict);
                

        }
    }
}
