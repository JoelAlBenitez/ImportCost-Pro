using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.LandedCosts
{
    public class LandedCostDetailViewModel
    {
        [Display(Name = "Producto")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Quantity { get; set; }

        [Display(Name = "FOB Original")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal OriginalTotalFob { get; set; }

        [Display(Name = "FOB Local")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal LocalTotalFob { get; set; }

        [Display(Name = "Flete Asignado")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AssignedFreight { get; set; }

        [Display(Name = "Seguro Asignado")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AssignedInsurance { get; set; }

        [Display(Name = "CIF")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalCif { get; set; }

        [Display(Name = "Arancel")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalTariff { get; set; }

        [Display(Name = "Impuesto Selectivo")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalSelectiveTax { get; set; }

        [Display(Name = "Tasa Servicio Aduanal")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalCustomsServiceFee { get; set; }

        [Display(Name = "ITBIS")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalItbis { get; set; }

        [Display(Name = "Gastos Locales Asignados")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AssignedLocalExpenses { get; set; }

        [Display(Name = "Costo Total Importado")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalImportedCost { get; set; }

        [Display(Name = "Costo Unitario Importado")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal UnitImportedCost { get; set; }

        [Display(Name = "Margen Deseado")]
        [DisplayFormat(DataFormatString = "{0:N2}%")]
        public decimal DesiredMargin { get; set; }

        [Display(Name = "Precio Venta Sugerido")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal SuggestedSalePrice { get; set; }
    }
}