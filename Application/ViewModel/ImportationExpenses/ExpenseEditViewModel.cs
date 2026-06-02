using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;


namespace ImportCost.ViewModels.ImportationExpenses
{
    public class ExpenseEditViewModel
    {
        //Llave primaria del gasto 
        [Required]
        public string ImportationExpenseId { get; set; } = string.Empty;

        //ID de la Orden 
        [Required]
        public string OrderId { get; set; } = string.Empty;

        // Propiedad de Solo Lectura
        [Display(Name = "Orden de Importación")]
        public string OrderNumberDisplay { get; set; } = string.Empty;

        //Campos editables con las mismas validaciones de la creación
        [Required(ErrorMessage = "Debe seleccionar un tipo de gasto.")]
        [Display(Name = "Tipo de Gasto")]
        public ExpenseType ExpenseType { get; set; }

        [Required(ErrorMessage = "El monto del gasto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        [Display(Name = "Monto")]
        public decimal ExpenseAmount { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una moneda.")]
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un método de distribución.")]
        [Display(Name = "Método de Distribución")]
        public DistributionMethod DistributionMethod { get; set; }

        [Required(ErrorMessage = "La fecha del gasto es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Gasto")]
        public DateTime ImportationExpenseDate { get; set; }

        // Diccionario para el Select de Monedas

        public Dictionary<int, string>? CurrenciesList { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}