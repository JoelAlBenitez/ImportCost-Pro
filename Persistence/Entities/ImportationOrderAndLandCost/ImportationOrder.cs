using Persistence.Entities.Enums;
using Persistence.Entities.OperationalCommercial;
using Persistence.Entities.FinancialCore;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationOrder
    {
        public required string OrderId { get; set; }

        public required int ImporterId { get; set; }

        public required int SupplierId { get; set; }

        public required int OriginCountryId { get; set; }

        public required int CurrencyId { get; set; }

        public required DateTime OrderDate { get; set; }

        public required TransportMode TransportMode { get; set; }
        public required OrderState OrderState { get; set; }

        //navigation properties

        public virtual Importers Importer { get; set; } = null!;

        public virtual Suppliers Supplier { get; set; } = null!;

        public virtual Country OriginCountry { get; set; } = null!;

        public virtual Currency Currency { get; set; } = null!;

        public virtual ICollection<ImportationOrderDetail> ImportationOrderDetails { get; set; } = new List<ImportationOrderDetail>();

        public virtual LandedCostSummary LandedCostSummary { get; set; } = null!;

        public virtual ICollection<ImportationExpense> ImportationExpenses { get; set; } = new List<ImportationExpense>();


    }
}
