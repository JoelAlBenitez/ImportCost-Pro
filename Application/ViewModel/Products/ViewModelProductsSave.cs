using Application.ViewModel.Select;
using System.ComponentModel.DataAnnotations;
namespace Application.ViewModel.Products
{
    public class ViewModelProductsSave  : IValidatableObject
    {

        public int Key { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre de producto válido que no supere los 150 caracteres.")]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Ingrese un código de referencia válido que no supere los 50 caracteres.")]
        [StringLength(50)]
        public required string CodeReference { get; set; }

        [Required(ErrorMessage = "Ingrese un precio unitario válido, mayor que 0.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ingrese un precio unitario mayor que 0.")]
        public required decimal UnitWeight { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoria valida del listado de categorias")]
        public required List<ViewModelSelectCategories> Categories { get; set; }

        [Required(ErrorMessage = "Seleccione una unidad de medida válida para el producto.")]
        public required List<ViewModelSelectUnit> Units { get; set; }

        [Required(ErrorMessage = "El país seleccionado no corresponde a un país válido.")]
        //public required List<ViewModelSelectCountries> countries { get; set;} //descomentar cuando se creen los servicios de paises

        [StringLength(250)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Seleccione un estado válido para el producto que intenta crear.")]
        public required bool State { get; set; }
        
        public required string CategoriesId { get; set; }
        public required int unit { get; set; }
        public required int CountryId { get; set; }
        public decimal? Large { get; set; }
        public decimal? Broad { get; set; }
        public decimal? High { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int field = 0;
            if (Large.HasValue && Large > 0) field++;
            if(High.HasValue && High > 0) field++;
            if(Broad.HasValue && Broad > 0) field++;
            if(field > 1 && field != 3)
            {
                yield return new ValidationResult(
                    "Si se ingresa un valor para el largo, alto o ancho los tres deben tener valores mayores que 0",
                    new[] {nameof(Large), nameof(High), nameof(Broad)}
                    );
            }
            if(Large < 0 || High <0 && Broad < 0)
            {
                yield return new ValidationResult(
                    "Ninguno de los valores de largo, alto o ancho puede tener un elemento menor a 0",
                    new[] { nameof(Large), nameof(High), nameof(Broad) }
                    );
            }
           
        }
    }
}