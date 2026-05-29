

using Persistence.Entities.OperationalCommercial;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationOrderDetail
    {
        public required string OrderDetailId { get; set; }

        public required string OrderId { get; set; }

        public required int ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal FOBUnitPrice { get; set; }

        public required decimal ExpectedProfitMargin { get; set; }

        //navigation properties
        public  Products Product { get; set; }
        public  ImportationOrder ImportationOrder { get; set; }
    }
}
