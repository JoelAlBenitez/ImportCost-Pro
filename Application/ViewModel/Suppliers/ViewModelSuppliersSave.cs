using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Suppliers
{
    public class ViewModelSuppliersSave
    {
        public int Key { get; set; }

        [Required(ErrorMessage =  "El nombre del proveedor es requerido, ingrese un valor valido en el mismo no mayor a 150 caracteres")]
        [StringLength(150,  MinimumLength = 3, ErrorMessage = "Ingrese un nombre de al menos 3 caracteres y que no supere los 150  caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país valido del listado de paises")]
        public required int CountryId {  get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un formato de correo eletronico valido")]
        [StringLength(100, MinimumLength = 7, ErrorMessage ="Ingrese un correo electronico con una longitud minima de 7 caracteres y que no supere los 100 caracteres ")]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "Ingrese un formato de telefonico valido")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Ingrese un número de teléfono de al menos 3 digitos  y que no supere los 20 caracteres")]
        public required string Phone {  get; set; }

        public  List<ViewModelSelectCountries>? Countries { get; set; }

        [Required(ErrorMessage = "Seleccione un estado valido para el suplidor que esta intentando operar")]
        public required bool State {  get; set; }

        [Required(ErrorMessage = "Debe seleccionar un moneda del listado valida")]
        public required int CurrencyId { get; set; }
        public List<ViewModelSelectCurrency>? Currencies { get; set; }
         
    }

}
