using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;
// IMPORTANTE: Asegúrate de tener los using de tus Enums
// using Persistence.Entities.Enums; 

namespace ImportCost.ViewModels.ImportationOrders
{
    public class ImportationOrderEditViewModel
    {
        //Campo oculto Para saber cuál era el ID antes de que el usuario lo edite y poder buscarlo
        [Required]
        public string OriginalOrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de orden es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El número de orden no puede superar los 30 caracteres.")]
        [Display(Name = "Número de Orden")]
        public string OrderId { get; set; } = string.Empty;

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
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una modalidad de transporte.")]
        [Display(Name = "Modalidad de Transporte")]
        public TransportMode TransportMode { get; set; }

        // El estado se envía a la vista solo para mostrarlo en una etiqueta
        [Display(Name = "Estado Actual")]
        public OrderState OrderState { get; set; }


        // Si esto es "true", la vista HTML pondrá los menús desplegables en "disabled"
        public bool IsCalculated { get; set; }

        // Diccionarios para Selects (Menús Desplegables)
        public Dictionary<int, string>? ImportersList { get; set; }
        public Dictionary<int, string>? SuppliersList { get; set; }
        public Dictionary<int, string>? CountriesList { get; set; }
        public Dictionary<int, string>? CurrenciesList { get; set; }
    }
}