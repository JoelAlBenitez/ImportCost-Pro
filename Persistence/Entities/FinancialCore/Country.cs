using Persistence.Entities.Base;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Entities.FinancialCore
{
    public class Country : BaseEntity<int, string>
    {
        public required string IsoCode { get; set; }

        /* 
        // Relaciones de navegación
        public ICollection<Suppliers>? Suppliers { get; set; }
        public ICollection<Importers>? Importers { get; set; }
        public ICollection<Products>? Products { get; set; }
        */



    }
}
