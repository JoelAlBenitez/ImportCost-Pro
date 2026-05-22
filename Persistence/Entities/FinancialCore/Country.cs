using Persistence.Entities.Base;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Entities.FinancialCore
{
    public class Country : BaseEntity<int, string>
    {
        public required string IsoCode { get; set; }

        /* 
        // Relaciones de navegación
        public virtual ICollection<Suppliers>? Suppliers { get; set; }
        public virtual ICollection<Importers>? Importers { get; set; }
        public virtual ICollection<Products>? Products { get; set; }
        */



    }
}
