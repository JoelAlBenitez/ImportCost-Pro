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

        public  Importers Importer { get; set; }

        public  Suppliers Supplier { get; set; }

        public  Country Country { get; set; }

        public  Currency Currency { get; set; }

        public  ICollection<ImportationOrderDetail> ImportationOrderDetails { get; set; }

        public  LandedCostSummary LandedCostSummary { get; set; }

        public  ICollection<ImportationExpense> ImportationExpenses { get; set; }


    }
}
