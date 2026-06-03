using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.OrderDetails
{
    public class OrderDetailEditViewModel
    {
        [Required]
        public string? OrderDetailId { get; set; }

        [Required]
        public string? OrderId { get; set; } 

        [Required]
        public int ProductId { get; set; }

        [Display(Name = "Producto")]
        public string? ProductName { get; set; } 
  
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        [Display(Name = "Cantidad a Importar")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario FOB es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio FOB debe ser mayor que 0.")]
        [Display(Name = "Precio Unitario FOB")]
        public decimal FOBUnitPrice { get; set; }

        [Required(ErrorMessage = "El margen de ganancia es obligatorio.")]
        [Range(0, 99.99, ErrorMessage = "El margen debe ser mayor o igual a 0 y menor que 100.")]
        [Display(Name = "Margen de Ganancia Esperado (%)")]
        public decimal ExpectedProfitMargin { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El Total FOB debe ser mayor que 0.")]
        public decimal TotalFOB { get; set; }
    }
}