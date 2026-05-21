using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Configurations.OperationalCommercial
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(k => k.Key);

            builder.Property(p => p.Key).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.State).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.Description).HasMaxLength(250);

            builder.HasOne(t => t.tariffCategories)
                .WithMany(p => p.Products)
                .HasForeignKey(t => t.tarrifCategoriesId)
                .OnDelete(DeleteBehavior.Cascade);

            /*builder.HasOne(c => c.Countrys)
             .WithMany(p => p.Products)
             .HasForeignLKey(c => c.countryId)
             .OnDelete(DeleteBehavior.Cascade);  
             */
        }
    }
}
