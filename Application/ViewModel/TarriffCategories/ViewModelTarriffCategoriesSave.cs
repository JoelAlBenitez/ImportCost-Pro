using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.TarriffCategories
{
    public class ViewModelTarriffCategoriesSave
    {
        
        [Required(ErrorMessage = "You must enter a valid tariff code no longer than 20 characters.")]
        [StringLength(20) ]
        public required string TarriffCode { get; set; }

        [Required(ErrorMessage = "You must enter a valid tariff name or description no longer than 150 characters.")]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Enter a valid tariff percentage, it must be between 0 and 100.")]
        [Range(0, 100)]
        public required decimal TarriffPorcetage { get; set; }

        [Required(ErrorMessage = "You must indicate a valid option for the application or non-application of ITBIS")]
        public required bool ITBIS { get; set; }

        [Required(ErrorMessage = "You must indicate a valid option for the application or non-application of Selective tax applies")]
        public required bool SelectiveTaxApplies {  get; set; }
        public decimal PorcentageTaxSelective { get; set; } //agregar condicional de si el impuesto selectivo esta marcado como aplicado

        [Required(ErrorMessage = "The tariff category must have a valid status.")]
        public required bool State {  get; set; }
        
    }
}
