namespace Application.DTOs.Orders;

using System;
using Persistence.Entities.Enums;

public class ImportationOrderCreateDTO
{
        public string OrderId { get; set; } = null!; // El PDF dice Max 30 caracteres

        public int ImporterId { get; set; }

        public int SupplierId { get; set; }

        public int OriginCountryId { get; set; }

        public int CurrencyId { get; set; }

        public DateTime OrderDate { get; set; }

        public TransportMode TransportMode { get; set; }
}
