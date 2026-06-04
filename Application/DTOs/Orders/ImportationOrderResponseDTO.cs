namespace Application.DTOs.Orders;

using System;
using Persistence.Entities.Enums;
public class ImportationOrderResponseDTO
{
    public string OrderId { get; set; } = null!;  

    public int ImporterId { get; set; }

    public int SupplierId { get; set; }

    public int OriginCountryId { get; set; }

    public int CurrencyId { get; set; }

    public string? ImporterName { get; set; }
    public string? SupplierName { get; set; }
    public string? OriginCountryName { get; set; }
    public string? CurrencyCode { get; set; }

    public DateTime OrderDate { get; set; }

    public TransportMode TransportMode { get; set; }

    public OrderState OrderState { get; set; }

    public decimal TotalFOB { get; set; }

    public decimal TotalImportationExpected { get; set; }

}
