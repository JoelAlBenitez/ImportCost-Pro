using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.OrderDetails
{
    public class OrderDetailViewModel
    {
        
        public string? OrderDetailId { get; set; }

        [Display(Name = "Producto")]
        public string? ProductName { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Quantity { get; set; }

        [Display(Name = "Precio Unitario FOB")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal FOBUnitPrice { get; set; }

        //CAMPOS CALCULADOS
        // (Cantidad * Precio Unitario FOB)
        [Display(Name = "FOB Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalFob { get; set; }

        // (Cantidad * Peso Unitario del Producto)
        [Display(Name = "Peso Total (kg)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalWeight { get; set; }

        // (Cantidad * Largo * Ancho * Alto)
        [Display(Name = "Volumen Total (cm³)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalVolume { get; set; }

        [Display(Name = "Margen Deseado (%)")]
        [DisplayFormat(DataFormatString = "{0:N2}%")]
        public decimal ExpectedProfitMargin { get; set; }
    }
}