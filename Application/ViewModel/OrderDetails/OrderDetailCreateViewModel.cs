using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.OrderDetails
{
    public class OrderDetailCreateViewModel
    {
        [Required(ErrorMessage = "El ID de la orden es obligatorio.")]
        public string OrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Display(Name = "Producto")]
        public int ProductId { get; set; }

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

        public List<ViewModelSelectProducts>? ProductsList { get; set; }
    }
}