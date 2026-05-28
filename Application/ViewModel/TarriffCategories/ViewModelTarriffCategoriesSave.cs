using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.TarriffCategories
{
    public class ViewModelTarriffCategoriesSave
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
        public decimal? PorcentageTaxSelective { get; set; }

        [Required(ErrorMessage = "La categoría de arancel debe tener un estado válido.")]
        public required bool State { get; set; }

    }
}