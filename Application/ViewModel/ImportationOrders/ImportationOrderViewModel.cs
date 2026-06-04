using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.ImportationOrders
{
    public class ImportationOrderViewModel
    {
        [Display(Name = "Número de Orden")]
        public string? OrderId { get; set; } 

        [Display(Name = "Importador")]
        public string? ImporterName { get; set; } 

        [Display(Name = "Proveedor")]
        public string? SupplierName { get; set; } 

        [Display(Name = "País de Origen")]
        public string? OriginCountryName { get; set; } 

        [Display(Name = "Moneda")]
        public string? CurrencyCode { get; set; } 

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Transporte")]
        public string? TransportMode { get; set; } 

        [Display(Name = "Estado")]
        public string? OrderState { get; set; } 

        
        [Display(Name = "FOB Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalFob { get; set; }

        [Display(Name = "Total Estimado de Importación")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal EstimatedTotalCost { get; set; }
    }
}