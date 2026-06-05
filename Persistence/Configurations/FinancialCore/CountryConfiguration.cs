using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.FinancialCore;

namespace Persistence.Configurations.FinancialCore
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Nombre de la tabla segn el documento
            builder.ToTable("Countries");

            // Llave primaria heredada de BaseEntityyy
            builder.HasKey(c => c.Key);

            // Configuración del Nombre del País
            // El documento dice: Requerido, máximo 150 caracteres (basado en recomendación de longitud razonable)
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Configuración del Código ISO
            // El documento dice: Requerido, 2 a 3 caracteres, Único
            builder.Property(c => c.IsoCode)
                .IsRequired()
                .HasMaxLength(3);

            // Índice único para el Código ISO para evitar repeticiones en la DB
            builder.HasIndex(c => c.IsoCode)
                .IsUnique();

            // Configuración del Estado
            // El documento dice: Por defecto Activo (true)
            builder.Property(c => c.State)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}