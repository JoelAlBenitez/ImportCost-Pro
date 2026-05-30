<<<<<<< HEAD
Ôªøusing Application.ViewModel.Select;
=======
using Application.ViewModel.Select;
using Persistence.Entities.Enums;
>>>>>>> origin/feature/FinancialCore
using System.ComponentModel.DataAnnotations;
namespace Application.ViewModel.Products
{
    public class ViewModelProductsSave  : IValidatableObject
    {

        public int Key { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre de producto v·lido que no supere los 150 caracteres.")]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Ingrese un cÛdigo de referencia v·lido que no supere los 50 caracteres.")]
        [StringLength(50)]
        public required string CodeReference { get; set; }

        [Required(ErrorMessage = "Ingrese un precio unitario v·lido, mayor que 0.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ingrese un precio unitario mayor que 0.")]
        public required decimal UnitWeight { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoria valida del listado de categorias")]
        public required List<ViewModelSelectCategories> Categories { get; set; }

<<<<<<< HEAD
        [Required(ErrorMessage = "Seleccione una unidad de medida v√°lida para el producto.")]
        public required List<ViewModelSelectUnit> Units { get; set; }
=======
        public decimal? Large { get; set; } = 0;
        public decimal? Broad { get; set; } = 0;
        public decimal? High { get; set; } = 0;

        [Required(ErrorMessage = "Seleccione una unidad de medida v·lida para el producto.")]
        public required UnitMesaurement unit { get; set; }
        //public List<ViewModelSelectUnit> UnitUnits { get; set; }
>>>>>>> origin/feature/FinancialCore

        [Required(ErrorMessage = "El pa√≠s seleccionado no corresponde a un pa√≠s v√°lido.")]
        //public required List<ViewModelSelectCountries> countries { get; set;} //descomentar cuando se creen los servicios de paises

        [StringLength(250)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Seleccione un estado v·lido para el producto que intenta crear.")]
        public required bool State { get; set; }
        
        public required string CategoriesId { get; set; }
<<<<<<< HEAD
        public required int unit { get; set; }
=======

        [Required(ErrorMessage = "El paÌs seleccionado no corresponde a un paÌs v·lido.")]
>>>>>>> origin/feature/FinancialCore
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
