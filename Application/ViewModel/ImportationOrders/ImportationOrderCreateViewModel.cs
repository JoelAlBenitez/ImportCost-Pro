using Application.ViewModel.Select;
using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.ImportationOrders
{
    public class ImportationOrderCreateViewModel
    {
        [Required(ErrorMessage = "El número de orden es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El número de orden no puede superar los 30 caracteres.")]
        [Display(Name = "Número de Orden")]
        public required string OrderId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un importador.")]
        [Display(Name = "Importador")]
        public int ImporterId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        [Display(Name = "Proveedor")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país de origen.")]
        [Display(Name = "País de Origen")]
        public int OriginCountryId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una moneda.")]
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "La fecha de la orden es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de la Orden")]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar una modalidad de transporte.")]
        [Display(Name = "Modalidad de Transporte")]
        public TransportMode TransportMode { get; set; }
        public List<ViewModelSelectImporters>? ImportersList { get; set; }
        public List<ViewModelSelectSuppliers>? SuppliersList { get; set; }
        public List<ViewModelSelectCountries>? CountriesList { get; set; }
        public List<ViewModelSelectCurrency>? CurrenciesList { get; set; }

        
    }
}