using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.TarriffCategories
{
    public class ViewModelTarriffCategoriesSave : IValidatableObject
    {

   
        [Required(ErrorMessage = "Debe ingresar un código de arancel válido que no supere los 20 caracteres.")]
        [StringLength(20)]
        public required string TarriffCode { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre o descripción de arancel válido que no supere los 150 caracteres.")]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Ingrese un porcentaje de arancel válido, debe estar entre 0 y 100.")]
        [Range(0, 100)]
        public required decimal TarriffPorcetage { get; set; }

        [Required(ErrorMessage = "Debe indicar una opción válida para la aplicación o no aplicación del ITBIS.")]
        public required bool ITBIS { get; set; }

        [Required(ErrorMessage = "Debe indicar una opción válida para la aplicación o no aplicación del Impuesto Selectivo.")]
        public required bool SelectiveTaxApplies { get; set; }

        [Range(0,100, ErrorMessage = "El porcentaje de impuesto selectivo debe encontrarseen el rango de 0 a 100 ")]
        public decimal? PorcentageTaxSelective { get; set; }

        [Required(ErrorMessage = "La categoría de arancel debe tener un estado válido.")]
        public required bool State { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectiveTaxApplies && PorcentageTaxSelective <= 0)
            {
                yield return new ValidationResult(
                    "Si el impuesto selectivo esta marcado como verdadero, el porcentaje del impuesto selectivo debe ser mayor a 0",
                    new[] { nameof(PorcentageTaxSelective) }
                    );
            }
            if(!SelectiveTaxApplies && PorcentageTaxSelective != 0)
            {
                yield return new ValidationResult(
                   "Si el impuestose selectivo no aplica, el porcentaje delimpuesto selectivo debe ser 0.",
                   new[] { nameof(PorcentageTaxSelective) }
                   );
            }            
        }
    }
}