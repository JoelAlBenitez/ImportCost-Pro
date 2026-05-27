namespace Application.DTOs.Orders;

using System;
using System.ComponentModel.DataAnnotations;
using Persistence.Entities.Enums;

public class ImportationOrderCreateDTO
{
        [Required(ErrorMessage = "El número de orden es requerido.")]
        [MaxLength(30, ErrorMessage = "El número de orden no puede superar los 30 caracteres.")]
        public string OrderId { get; set; } = null!; // El PDF dice Max 30 caracteres

        [Required(ErrorMessage = "Debe seleccionar un importador.")]
        public int ImporterId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país de origen.")]
        public int OriginCountryId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una moneda.")]
        [MaxLength(3)]
        public string CurrencyId { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de la orden es requerida.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "La modalidad de transporte es requerida.")]
        public TransportMode TransportMode { get; set; }
}
