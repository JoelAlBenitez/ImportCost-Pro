namespace Application.DTOs.Expenses;

using System;
using Persistence.Entities.Enums;

public class ImportationExpenseResponseDTO
{
    public string ImportationExpenseId { get; set; }
    public string OrderId { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public decimal ExpenseAmount { get; set; }
    public int CurrencyId { get; set; }
    public DistributionMethod DistributionMethod { get; set; }
    public DateTime ExpenseDate { get; set; }
}
