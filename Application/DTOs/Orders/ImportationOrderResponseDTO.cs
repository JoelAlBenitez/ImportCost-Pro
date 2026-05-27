namespace Application.DTOs.Orders;

using System;
using System.ComponentModel.DataAnnotations;
using Persistence.Entities.Enums;
public class ImportationOrderResponseDTO
{
    public string OrderId { get; set; } = null!; // El PDF dice Max 30 caracteres

    public int ImporterId { get; set; }

    public int SupplierId { get; set; }

    public int OriginCountryId { get; set; }

    public string CurrencyId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public TransportMode TransportMode { get; set; }

    public OrderState OrderState { get; set; }

    public decimal TotalFOB { get; set; }

    public decimal TotalImportationExpected { get; set; }

}
