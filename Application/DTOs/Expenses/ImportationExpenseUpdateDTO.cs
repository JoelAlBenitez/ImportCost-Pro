namespace Application.DTOs.Expenses;

using System;
using Persistence.Entities.Enums;

public class ImportationExpenseUpdateDTO
{
    public string ImportationExpenseId { get; set; } = null!; // El PDF dice Max 30 caracteres
    public ExpenseType ExpenseType { get; set; }
    public decimal ExpenseAmount { get; set; }
    public int CurrencyId { get; set; }
    public DistributionMethod DistributionMethod { get; set; }
    public DateTime ExpenseDate { get; set; }

}
