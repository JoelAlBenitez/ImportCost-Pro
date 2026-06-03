using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Countries
{
    public class ViewModelCountrySave
    {
        public required int Key { get; set; }

        [Required(ErrorMessage = "El nombre del país es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El código ISO es obligatorio.")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "El código ISO debe tener entre 2 y 3 caracteres.")]
        public required string IsoCode { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido.")]
        public required bool State { get; set; }
    }
}
