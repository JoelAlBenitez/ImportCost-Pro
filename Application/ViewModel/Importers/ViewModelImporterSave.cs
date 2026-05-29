using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Importers
{
    public class ViewModelImporterSave
    {

        public int Key { get; set; }

        [Required(ErrorMessage = "Nombre inválido o demasiado largo, por favor ingrese un nombre válido de no más de 150 caracteres.")]
        [StringLength(150, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Required(ErrorMessage = "Identificación inválida o demasiado larga, por favor ingrese una identificación válida de no más de 20 caracteres.")]
        public required string Identifcation { get; set; }

        [Required(ErrorMessage = "País inválido, por favor seleccione un país válido.")]
        public required int countryId { get; set; }
        public List<ViewModelSelectCountries>? Countries { get; set; }

        [StringLength(20, MinimumLength = 0)]
        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(250, MinimumLength = 0)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido para el importador que intenta crear.")]
        public required bool State { get; set; }
    }
}