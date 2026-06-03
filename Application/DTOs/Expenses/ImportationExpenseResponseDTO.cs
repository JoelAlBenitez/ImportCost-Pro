namespace Application.DTOs.Expenses;

using Application.Services.Result;
using Persistence.Entities.Enums;
using System;

public class ImportationExpenseResponseDTO
{
    public required string ImportationExpenseId { get; set; }
    public required string OrderId { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public decimal ExpenseAmount { get; set; }
    public int CurrencyId { get; set; }
    public DistributionMethod DistributionMethod { get; set; }
    public DateTime ExpenseDate { get; set; }
}
