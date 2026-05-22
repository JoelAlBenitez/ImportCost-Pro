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
    }
}
