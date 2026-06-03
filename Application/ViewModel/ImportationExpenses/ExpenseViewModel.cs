using System.ComponentModel.DataAnnotations;

namespace ImportCost.ViewModels.ImportationExpenses
{
    public class ExpenseViewModel
    {
        public string ImportationExpenseId { get; set; } = string.Empty;

       
        [Display(Name = "Tipo de Gasto")]
        public string ExpenseType { get; set; } = string.Empty;

        [Display(Name = "Monto")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ExpenseAmount { get; set; }

        
        [Display(Name = "Moneda")]
        public string CurrencyCode { get; set; } = string.Empty;

        [Display(Name = "Método de Distribución")]
        public string DistributionMethod { get; set; } = string.Empty;

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ImportationExpenseDate { get; set; }
        public string? OrderId { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}