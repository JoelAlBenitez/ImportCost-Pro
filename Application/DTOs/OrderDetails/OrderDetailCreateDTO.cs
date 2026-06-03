namespace Application.DTOs.OrderDetails;

using System;
using Persistence.Entities.Enums;

public class OrderDetailCreateDTO
{
        public string OrderId { get; set; } = null!;

        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal FOBUnitPrice { get; set; }
        public decimal ExpectedProfitMargin { get; set; }
}
