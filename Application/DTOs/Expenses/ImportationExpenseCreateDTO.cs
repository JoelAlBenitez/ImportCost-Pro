namespace Application.DTOs.Expenses;

using System;
using Persistence.Entities.Enums;

public class ImportationExpenseCreateDTO
{
    public string OrderId { get; set; } = null!;
    public ExpenseType ExpenseType { get; set; }
    public decimal ExpenseAmount { get; set; }
    public int CurrencyId { get; set; }
    public DistributionMethod DistributionMethod { get; set; }
    public DateTime ExpenseDate { get; set; }

}
