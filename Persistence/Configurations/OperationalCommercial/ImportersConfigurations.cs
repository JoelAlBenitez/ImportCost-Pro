using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Configurations.OperationalCommercial
{
    public class ImportersConfigurations : IEntityTypeConfiguration<Importers>
    {
        public void Configure(EntityTypeBuilder<Importers> builder)
        {
            builder.ToTable("Importers");
       
            builder.HasKey(i => i.Key);
            builder.Property(i => i.Name).IsRequired().HasMaxLength(150);
            builder.Property(i => i.State).IsRequired().HasDefaultValue(1);
            builder.Property(i => i.Identification).IsRequired().HasMaxLength(20);
            builder.Property(i => i.Phone).HasMaxLength(20);
            builder.Property(i => i.Email).HasMaxLength(100);
            builder.Property(i => i.Address).HasMaxLength(250);
            builder.Property(i => i.State).IsRequired().HasDefaultValue(true);
            //builder.Property(i => i.countryId).IsRequired();
            /*
             builder.HasOne(c => c.Countrys)
             .WithMany(i => i.Impoerters)
             .HasForeighKey(i => i.countryId)
             .OnDelete(DeleteBehavior.Cascade);
             */
           
        }
    }
}
