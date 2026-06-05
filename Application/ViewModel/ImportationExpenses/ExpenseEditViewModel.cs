using Application.ViewModel.Select;
using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;


namespace ImportCost.ViewModels.ImportationExpenses
{
    public class ExpenseEditViewModel
    { 
        [Required(ErrorMessage = "El ID del gasto es obligatorio.")]
        public string ImportationExpenseId { get; set; } = string.Empty;
 
        [Required(ErrorMessage = "El identificador de la orden es obligatorio.")]
        public string OrderId { get; set; } = string.Empty;

        [Display(Name = "Orden de Importación")]
        public string OrderNumberDisplay { get; set; } = string.Empty;

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
        public List<ViewModelSelectCurrency> CurrenciesList { get; set; } = new();

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Gasto")]
        public DateTime ExpenseDate { get; set; } = DateTime.Today;
    }
}