namespace Application.DTOs.Expenses;

using System;
using System.ComponentModel.DataAnnotations;
using Persistence.Entities.Enums;

public class ImportationExpenseUpdateDTO
{
    [Required(ErrorMessage = "El ID del gasto es requerido.")]
    [MaxLength(30, ErrorMessage = "El ID del gasto no puede superar los 30 caracteres.")]
    public string ImportationExpenseId { get; set; } = null!; // El PDF dice Max 30 caracteres

    [Required(ErrorMessage = "Debe seleccionar un tipo de Gasto.")]
    public ExpenseType ExpenseType { get; set; }

    [Required(ErrorMessage = "Debe Digitar un Monto valido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
    public decimal ExpenseAmount { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una Moneda.")]
    [MaxLength(3)]
    public string CurrencyId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un método de distribución.")]
    public DistributionMethod DistributionMethod { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una Fecha Valida.")]
    public DateTime ExpenseDate { get; set; }

}
