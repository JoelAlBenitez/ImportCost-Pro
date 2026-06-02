using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.ExchangeRates
{
    public class ViewModelExchangeRateSave
    {
        public int Key { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una moneda de origen.")]
        public int SourceCurrencyId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una moneda de destino.")]
        public int DestinationCurrencyId { get; set; }

        // Propiedades para llenar los <select> en la vista
        public List<ViewModelSelectCurrency>? Currencies { get; set; }

        [Required(ErrorMessage = "El valor de la tasa es obligatorio.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El valor de la tasa debe ser mayor que 0.")]
        public decimal RateValue { get; set; }

        [Required(ErrorMessage = "La fecha de vigencia es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido.")]
        public bool State { get; set; }
    }
}
