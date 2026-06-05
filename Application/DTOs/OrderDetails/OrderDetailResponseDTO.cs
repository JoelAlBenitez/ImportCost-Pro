namespace Application.DTOs.OrderDetails;

using System;
using Persistence.Entities.Enums;

public class OrderDetailResponseDTO
{
    public string? OrderId { get; internal set; }
    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal FOBUnitPrice { get; set; }

    public decimal TotalFOB { get; set; }

    public decimal TotalWeight { get; set; }

    public decimal TotalVolume { get; set; }

    public decimal ExpectedProfitMargin { get; set; }
    public string? OrderDetailId { get; internal set; }
    public string? ProductName { get; internal set; }

}
