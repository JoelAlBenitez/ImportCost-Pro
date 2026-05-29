using Persistence.Entities.OperationalCommercial;


namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class LandedCostDetail
    {
        public required string LandedCostDetailId { get; set; }
        public required string LandedCostSummaryId { get; set; }

        public required int ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal OriginalFOB { get; set; }

        public required decimal LocalFob { get; set; }

        public required decimal AssignedFreight { get; set; }

        public required decimal AssignedInsurance { get; set; }

        public required decimal Cif { get; set; }

        public required decimal Tariff { get; set; }

        public required decimal SelectiveTax { get; set; }

        public required decimal CustomsServiceFee { get; set; }

        public required decimal Itbis { get; set; }

        public required decimal AssignedLocalExpenses { get; set; }

        public required decimal TotalImportedCost { get; set; }

        public required decimal UnitImportedCost { get; set; }

        public required decimal DesiredMargin { get; set; }

        public required decimal SuggestedSalePrice { get; set; }

        //navigation properties

        public  LandedCostSummary LandedCostSummary { get; set; }

        public  Products Product { get; set; }


    }
}
