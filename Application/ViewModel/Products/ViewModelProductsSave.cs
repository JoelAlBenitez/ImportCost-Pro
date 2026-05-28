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

        [Required(ErrorMessage = "Seleccione una categoría arancelaria válida.")]
        public required List<ViewModelSelectCategories> Categories { get; set; }

        public decimal? Large { get; set; } = 0;
        public decimal? Broad { get; set; } = 0;
        public decimal? High { get; set; } = 0;

        public string ValidateUnitMessaurent()
        {
            int fullFields = 0;
            if ((Large.HasValue && Large > 0) || Large <= 0) fullFields++;
            if ((Broad.HasValue && Broad > 0) || Broad <= 0) fullFields++;
            if ((High.HasValue && High > 0) || High <= 0) fullFields++;
            if (fullFields > 0 && fullFields < 3)
                return "Si ingresa un valor de (largo, ancho o alto), debe completar los tres campos secuencialmente para registrar el producto. Estos valores deben ser mayores que 0.";
            return null!;
        }

        
        [Required(ErrorMessage = "Seleccione una unidad de medida válida para el producto.")]
        public required UnitMesaurement unit { get; set; }

        [Required(ErrorMessage = "Por favor, seleccione un país de origen válido para el producto que intenta crear.")]
        //public required List<ViewModelSelectCountries> countries { get; set;} //descomentar cuando se creen los servicios de paises

        [StringLength(250)]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Seleccione un estado válido para el producto que intenta crear.")]
        public required bool State { get; set; }

        //
        [Required(ErrorMessage = "La categoría seleccionada no corresponde a una categoría válida.")]
        public required string CategoriesId { get; set; }

        //
        [Required(ErrorMessage = "El país seleccionado no corresponde a un país válido.")]
        public required int CountryId { get; set; }

    }
}