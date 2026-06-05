using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Importers
{
    public class ViewModelImporterSave
    {

        public int Key { get; set; }

        [Required(ErrorMessage = "Nombre inválido o demasiado largo, por favor ingrese un nombre válido de no más de 150 caracteres.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre del importador debe tener minimo 3 caracteres y   no  puede ser mayor a 20 caracteres")]
        public required string Name { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "La identificación debe tener un minimo de 3 digitos y no se mayor a 20. ")]
        [Required(ErrorMessage = "Identificación inválida o demasiado larga, por favor ingrese una identificación válida de no más de 20 caracteres.")]
        public required string Identifcation { get; set; }

        [Required(ErrorMessage = "País inválido, por favor seleccione un país válido.")]
        public required int countryId { get; set; }
        public List<ViewModelSelectCountries>? Countries { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Si ingresa un numero telefonico el mismo debe tener minimo 3 digitos y no puede ser mayor a 20 digitios")]
        [Phone(ErrorMessage = "El numero telefonico ingresado no tiene un formato valido")]
        public string? PhoneNumber { get; set; }

        [StringLength(100, MinimumLength = 11, ErrorMessage = "Si ingresa un numero email el mismo debe tener minimo 11 caracteres y no puede ser mayor a 10 caracteres")]
        [EmailAddress(ErrorMessage = "El email ingresado no tine un formatov valido")]
        public string? Email { get; set; }

        [StringLength(250, MinimumLength = 10, ErrorMessage = "Si ingresa una  direccion la misma debe tener minimo 10 caracteres y no puede ser mayor a 250 caracteres")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado válido para el importador que intenta crear.")]
        public required bool State { get; set; }
    }
}
