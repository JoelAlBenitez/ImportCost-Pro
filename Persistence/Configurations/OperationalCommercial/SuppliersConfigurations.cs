using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Configurations.OperationalCommercial
{
    internal class SuppliersConfigurations : IEntityTypeConfiguration<Suppliers>
    {
        public void Configure(EntityTypeBuilder<Suppliers> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(s => s.Key);

            builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
            builder.Property(c => c.countryId).IsRequired();

            builder.Property(s => s.Email).HasMaxLength(100);
            builder.Property(s => s.Phone).HasMaxLength(20);
            builder.Property(s => s.State).IsRequired().HasDefaultValue(true);
            builder.Property(s => s.MainCurrencyId).IsRequired();

            
             builder.HasOne(c => c.Country)
            .WithMany(s => s.Suppliers)
            .HasForeignKey(s => s.countryId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.MainCurrency)
            .WithMany(s => s.Suppliers)
            .HasForeignKey(s => s.MainCurrencyId).
            OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
