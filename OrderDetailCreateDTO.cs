using System;

public class OrderDetailCreateDTO
{
        [Required(ErrorMessage = "El número de orden es requerido.")]
        [MaxLength(30, ErrorMessage = "El número de orden no puede superar los 30 caracteres.")]
        public string OrderId { get; set; } = null!; // El PDF dice Max 30 caracteres

        [Required(ErrorMessage = "Debe seleccionar un Producto.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Debe colocar un numero mayor que 0.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Debe colocar un numero mayor que 0.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio FOB debe ser mayor que 0..")]
    public decimal FOBUnitPrice { get; set; }

        [Required(ErrorMessage = "Debe colocar un numero mayor que 0 y menor o igual que 100.")]
    [Range(0, 99.99, ErrorMessage = "El margen debe estar entre 0 y menor a 100.")]
    public decimal ExpectedProfitMargin { get; set; }
}
