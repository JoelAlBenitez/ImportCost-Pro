using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.FinancialCore;

namespace Persistence.Configurations.FinancialCore
{
    public class TaxConfigurationConfig : IEntityTypeConfiguration<TaxConfiguration>
    {
        public void Configure(EntityTypeBuilder<TaxConfiguration> builder)
        {
            builder.ToTable("TaxConfigurations");

            builder.HasKey(t => t.Key);

            builder.Property(t => t.Name)
                   .HasMaxLength(150)
                   .IsRequired();

            // Configuración de porcentajes con precisión decimal
            builder.Property(t => t.GeneralItbisPercentage)
                   .IsRequired()
                   .HasPrecision(18, 4);

            builder.Property(t => t.CustomsServiceRatePercentage)
                   .IsRequired()
                   .HasPrecision(18, 4);

            builder.Property(t => t.State)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}