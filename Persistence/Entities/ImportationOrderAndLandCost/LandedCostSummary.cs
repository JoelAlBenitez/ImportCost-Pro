

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class LandedCostSummary
    {
        public required string LandedCostSummaryId { get; set; }

        public required string OrderId { get; set; }

        public required int LocalCurrencyUsed { get; set; }

        public required decimal ExchangeRate { get; set; }

        public required decimal OriginalTotalFob { get; set; }
        public required decimal LocalTotalFob { get; set; }

        public required decimal TotalFreight { get; set; }

        public required decimal TotalInsurance { get; set; }

        public required decimal TotalCif { get; set; }

        public required decimal TotalTariff { get; set; }

        public required decimal TotalSelectiveTax { get; set; }

        public required decimal TotalCustomsServiceFee { get; set; }

        public required decimal TotalItbis { get; set; }

        public required decimal TotalLocalExpenses { get; set; }

        public required decimal TotalImportationCost { get; set; }

        public required decimal TotalImportedQuantity { get; set; }

        //navigation properties

        //antes: public required ImportationOrder ImportationOrder { get; set; }
        public virtual ImportationOrder ImportationOrder { get; set; } = null!;

        public virtual ICollection<LandedCostDetail> LandedCostDetails { get; set; } = new List<LandedCostDetail>();

    }
}
