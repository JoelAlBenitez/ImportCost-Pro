using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.FinancialCore;

namespace Persistence.Configurations.FinancialCore
{
    public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable("ExchangeRates");
            builder.HasKey(e => e.Key);
            builder.Property(e => e.RateValue).IsRequired().HasPrecision(18, 4);
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.State).HasDefaultValue(true);
            builder.HasIndex(e => new { e.SourceCurrencyId, e.DestinationCurrencyId, e.EffectiveDate })
                .IsUnique();


            // relacion con Moneda Origennn
            builder.HasOne(e => e.SourceCurrency)
                   .WithMany(e => e.ExchangeRatesSource)
                   .HasForeignKey(e => e.SourceCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacion con Moneda Destinoo
            builder.HasOne(e => e.DestinationCurrency)
                   .WithMany(e => e.ExchangeRatesDestination)
                   .HasForeignKey(e => e.DestinationCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

        }


    }
}