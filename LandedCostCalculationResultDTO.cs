using System;

public class LandedCostDetailDTO
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal OriginalTotalFob { get; set; }
    public decimal LocalTotalFob { get; set; }
    public decimal TotalFreight { get; set; }
    public decimal TotalInsurance { get; set; }
    public decimal TotalCif { get; set; }
    public decimal TotalTariff { get; set; }
    public decimal TotalSelectiveTax { get; set; }
    public decimal TotalCustomsServiceFee { get; set; }
    public decimal TotalItbis { get; set; }
    public decimal TotalLocalExpenses { get; set; }
    public decimal TotalImportationCost { get; set; }
    public decimal TotalImportedQuantity { get; set; }
    public decimal DesiredMargin { get; set; }
    public decimal SuggestedSalePrice { get; set; }

}

public class LandedCostSummaryDTO
{
    public string LandedCostSummaryId { get; set; }

    public string OrderId { get; set; }

    public string LocalCurrencyUsed { get; set; }

    public decimal ExchangeRate { get; set; }

    public decimal OriginalTotalFob { get; set; }
    public decimal LocalTotalFob { get; set; }

    public decimal TotalFreight { get; set; }

    public decimal TotalInsurance { get; set; }

    public decimal TotalCif { get; set; }

    public decimal TotalTariff { get; set; }

    public decimal TotalSelectiveTax { get; set; }

    public decimal TotalCustomsServiceFee { get; set; }

    public decimal TotalItbis { get; set; }

    public decimal TotalLocalExpenses { get; set; }

    public decimal TotalImportationCost { get; set; }

    public decimal TotalImportedQuantity { get; set; }
}
