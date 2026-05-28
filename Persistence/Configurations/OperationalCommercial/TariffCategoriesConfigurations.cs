using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Configurations.OperationalCommercial
{
    public class TariffCategoriesConfigurations : IEntityTypeConfiguration<TariffCategories>
    {
        public void Configure(EntityTypeBuilder<TariffCategories> builder)
        {
            builder.ToTable("TariffCategories");
            builder.HasKey(t => t.Key);

            builder.Property(t => t.Key).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
            builder.Property(t => t.PorcentageTariff).IsRequired().HasMaxLength(100);
            builder.Property(t => t.ITBIS).IsRequired();
            builder.Property(t => t.State).IsRequired().HasDefaultValue(true); 

        }
    }
}
