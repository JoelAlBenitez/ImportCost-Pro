using Application.ViewModel.Select;
using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;
namespace Application.ViewModel.Products
{
    public class ViewModelProductsSave
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

        public decimal? Large { get; set; } = 0;
        public decimal? Broad { get; set; } = 0;
        public decimal? High { get; set; } = 0;

        [Required(ErrorMessage = "Seleccione una unidad de medida válida para el producto.")]
        public required UnitMesaurement unit { get; set; }
        //public List<ViewModelSelectUnit> UnitUnits { get; set; }

        //public required List<ViewModelSelectCountries> countries { get; set;} //descomentar cuando se creen los servicios de paises

        [StringLength(250)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Seleccione un estado válido para el producto que intenta crear.")]
        public required bool State { get; set; }
        
        public required string CategoriesId { get; set; }

        [Required(ErrorMessage = "El país seleccionado no corresponde a un país válido.")]
        public required int CountryId { get; set; }

    }
}