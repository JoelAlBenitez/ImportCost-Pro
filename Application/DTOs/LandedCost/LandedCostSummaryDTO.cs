namespace Application.DTOs.LandedCost;

using System;
using Persistence.Entities.Enums;

public class LandedCostSummaryDTO
{
    public string LandedCostSummaryId { get; set; }

    public string OrderId { get; set; }

    public int LocalCurrencyUsed { get; set; }

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

    public List<LandedCostDetailDTO> ProductDetails { get; set; } = new();
}
