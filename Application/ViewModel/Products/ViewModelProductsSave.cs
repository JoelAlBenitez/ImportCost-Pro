using Application.ViewModel.Select;
using Persistence.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Products
{
    public class ViewModelProductsSave
    {
        //valid name 
        [Required (ErrorMessage = "Enter a valid product name no longer than 150 characters.")]
        [StringLength(150)]
        public required string Name { get; set; }

        //code reference
        [Required (ErrorMessage = " Enter a valid code reference  no longer than 50 characters.")]
        [StringLength (50)]
        public required string CodeReference { get; set; }

        //unit 
        [Required (ErrorMessage = "Enter a valid unit price, greater than 0.")]
        [Range (0.01, double.MaxValue, ErrorMessage = "Enter a unit price greater than 0.")]
        public required decimal UnitWeight { get; set; }

        //List categories 
        [Required (ErrorMessage = "Select a valid tariff category.")]
        public required List<ViewModelSelectCategories> Categories { get; set; }

        //possible values null
        public decimal? Large { get; set; } = 0;
        public decimal? Broad { get; set; } = 0;
        public decimal? High { get; set; } = 0;

        public string ValidateUnitMessaurent()
        {
            int fullFields = 0;
            if ((Large.HasValue && Large > 0) || Large <= 0 ) fullFields++;
            if ((Broad.HasValue && Broad > 0 )|| Broad <= 0) fullFields++;
            if((High.HasValue && High > 0) || High <= 0) fullFields++;
            if (fullFields > 0 && fullFields < 3)
                return "If you enter a (length, width, or height) value, you must complete all three fields sequentially to register the product. These values ​​must be greater than 0.";
            return null!;
        }

        //unit mesauremnt 
        [Required (ErrorMessage = "Select a valid unit of measurement for the product.")]
        public required UnitMesaurement unit {get; set;}
          
        //list countries 
        [Required (ErrorMessage = "Please select a valid country of origin for the product you are trying to create.")]
        public required List<ViewModelSelectCountries> countries { get; set;}

        //possible value null
        [StringLength(250)]
        public string? Description { get; set; }

        // state  product -> default true values
        [Required(ErrorMessage = "Select a valid state for the product you are trying to create.")]
        public required bool State { get; set; } = true;
    }
}
