using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.TaxConfigurations
{
    public class ViewModelTaxConfigurationSave
    {
        public int Key { get; set; }

        [Required(ErrorMessage = "El porcentaje de ITBIS general es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El ITBIS debe estar entre 0 y 100.")]
        public decimal GeneralItbisPercentage { get; set; }

        [Required(ErrorMessage = "El porcentaje de tasa de servicio aduanero es obligatorio.")]
        [Range(0, 100, ErrorMessage = "La tasa de servicio aduanero debe estar entre 0 y 100.")]
        public decimal CustomsServiceRatePercentage { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido.")]
        public bool State { get; set; }
    }
}
