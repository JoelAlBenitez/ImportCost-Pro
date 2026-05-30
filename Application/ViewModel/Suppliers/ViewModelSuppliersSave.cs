using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Suppliers
{
    public class ViewModelSuppliersSave
    {
        public int Key { get; set; }

        [Required(ErrorMessage =  "El nombre del proveedor es requerido, ingrese un valor valido en el mismo no mayor a 150 caracteres")]
        [StringLength(150,  MinimumLength = 3)]
        public required string Name { get; set; }
        public required int CountryId {  get; set; }

        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public required string Phone {  get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país valido del listado de paises")]
        public required List<ViewModelSelectCountries> Countries { get; set; }

        [Required(ErrorMessage = "Seleccione un estado valido para el suplidor que esta intentando operar")]
        public required bool State {  get; set; }

        public required int CurrencyId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un moneda del listado valida")]
        public required List<ViewModelSelectCurrency> Currencies { get; set; }
         
    }

}
