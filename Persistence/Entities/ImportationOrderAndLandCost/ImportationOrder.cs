using Persistence.Entities.OperationalCommercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public required string TransportMode { get; set; }

        public required string OrderState { get; set; }

        //navigation properties

        public required Importers Importer { get; set; }

        public required Suppliers Supplier { get; set; }

        //public required Countries Countries { get; set; }

        //public required Currencies Currencies { get; set; }

        public required ICollection<ImportationOrderDetail> ImportationOrderDetails { get; set; }

        public required LandedCostSummary LandedCostSummary { get; set; }

        public required ICollection<ImportationExpense> ImportationExpenses { get; set; }


    }
}
