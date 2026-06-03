using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Currencies
{
    public class ViewModelCurrencySave
    {
        public int Key { get; set; }

        [Required(ErrorMessage = "El nombre de la moneda es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El código ISO es obligatorio.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código ISO debe tener exactamente 3 caracteres.")]
        public required string IsoCode { get; set; }

        [Required(ErrorMessage = "El símbolo es obligatorio.")]
        [StringLength(10, ErrorMessage = "El símbolo no puede exceder los 10 caracteres.")]
        public required string Symbol { get; set; }

        [Required(ErrorMessage = "Debe indicar si es la moneda local.")]
        public required bool IsLocalCurrency { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido.")]
        public required bool State { get; set; }
    }
}
