using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.LandedCosts
{
    public class LandedCostSummaryViewModel
    {
        public string? OrderId { get; set; } 

        [Display(Name = "Moneda Local Usada")]
        public string? LocalCurrencyUsed { get; set; } 

        [Display(Name = "Tasa de Cambio Usada para la Orden")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "FOB Total Original")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal OriginalTotalFob { get; set; }

        [Display(Name = "FOB Total Local")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal LocalTotalFob { get; set; }

        [Display(Name = "Flete Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalFreight { get; set; }

        [Display(Name = "Seguro Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalInsurance { get; set; }

        [Display(Name = "CIF Total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalCif { get; set; }

        [Display(Name = "Total Arancel")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalTariff { get; set; }

        [Display(Name = "Total Impuesto Selectivo")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalSelectiveTax { get; set; }

        [Display(Name = "Total Tasa Servicio Aduanal")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalCustomsServiceFee { get; set; }

        [Display(Name = "Total ITBIS")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalItbis { get; set; }

        [Display(Name = "Total Gastos Locales")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalLocalExpenses { get; set; }

        [Display(Name = "Costo Total de Importación")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalImportationCost { get; set; }

        [Display(Name = "Cantidad Total Importada")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalImportedQuantity { get; set; }

        // La lista con el desglose por producto que se dibujará en la tabla principal
        public List<LandedCostDetailViewModel> ProductDetails { get; set; } = new List<LandedCostDetailViewModel>();
    }
}