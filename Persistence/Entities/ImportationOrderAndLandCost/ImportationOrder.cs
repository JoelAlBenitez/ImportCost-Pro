using Persistence.Entities.Enums;
using Persistence.Entities.OperationalCommercial;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationOrder
    {
        public required string OrderId { get; set; }

        public required int ImporterId { get; set; }

        public required int SupplierId { get; set; }

        public required int OriginCountryId { get; set; }

        public required string CurrencyId { get; set; }

        public required DateTime OrderDate { get; set; }

        public required TransportMode TransportMode { get; set; }
        public required OrderState OrderState { get; set; }

        //navigation properties

        public virtual Importers Importer { get; set; } = null!;

        public virtual Suppliers Supplier { get; set; } = null!;

        //public required Countries Countries { get; set; }

        //public required Currencies Currencies { get; set; }

        public required ICollection<ImportationOrderDetail> ImportationOrderDetails { get; set; }

        public virtual LandedCostSummary LandedCostSummary { get; set; } = null!;

        public required ICollection<ImportationExpense> ImportationExpenses { get; set; }


    }
}
