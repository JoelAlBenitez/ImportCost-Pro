using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Products
{
    public class ViewModelProductsSave
    {
        [Required (ErrorMessage = "Enter a valid product name no longer than 150 characters.")]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required (ErrorMessage = " Enter a valid code reference  no longer than 50 characters.")]
        [StringLength (50)]
        public required string CodeReference { get; set; }

        [Required (ErrorMessage = "Enter a valid unit price, greater than 0.")]
        [Range (0.01, double.MaxValue, ErrorMessage = "Enter a unit price greater than 0.")]
        public required decimal UnitWeight { get; set; }

        [Required (ErrorMessage = "Select a valid tariff category.")]
        public required int TarriffCategoriesId { get; set; }

        public decimal? Large { get; set; }
        public decimal? Broad { get; set; }
        public decimal? High {  get; set; }

        /*[Required (ErrorMessage = "Select a valid unit of measurement for the product.")];
         * public required Unit unit {get; set;}
         * 
         * [Required (ErrorMessage = "Please select a valid country of origin for the product you are trying to create.")]
         * public required int countryId {get; set;}
         */

        public string? Description { get; set; }

        [Required (ErrorMessage = "Select a valid state for the product you are trying to create.")]
        public required bool State { get; set; } = true;
    }
}
