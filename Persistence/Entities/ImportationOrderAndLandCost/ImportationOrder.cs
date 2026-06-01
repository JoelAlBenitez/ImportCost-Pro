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

        public  Importers Importer { get; set; } = null!;

        public  Suppliers Supplier { get; set; } = null!;

        public  Country Country { get; set; } = null!;

        public  Currency Currency { get; set; } = null!;

        public  ICollection<ImportationOrderDetail> ImportationOrderDetails { get; set; } = new List<ImportationOrderDetail>();

        public  LandedCostSummary LandedCostSummary { get; set; } = null!;

        public  ICollection<ImportationExpense> ImportationExpenses { get; set; } = new List<ImportationExpense>();


    }
}
