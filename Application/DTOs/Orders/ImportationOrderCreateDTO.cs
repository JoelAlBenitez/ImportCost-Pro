namespace Application.DTOs.Orders;

using System;
using Persistence.Entities.Enums;

public class ImportationOrderCreateDTO
{
        public required string OrderId { get; set; } 

        public int ImporterId { get; set; }

        public int SupplierId { get; set; }

        public int OriginCountryId { get; set; }

        public int CurrencyId { get; set; }

        public DateTime OrderDate { get; set; }

        public TransportMode TransportMode { get; set; }
}
