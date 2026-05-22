using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationOrderDetail
    {
        public required string OrderDetailId { get; set; }

        public required string OrderId { get; set; }

        public required string ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal FOBUnitPrice { get; set; }

        public required decimal ExpectedProfitMargin { get; set; }
    }
}
