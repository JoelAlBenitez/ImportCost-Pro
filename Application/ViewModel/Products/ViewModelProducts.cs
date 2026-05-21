using Application.ViewModel.Base;

namespace Application.ViewModel.Products
{
    public class ViewModelProducts : BaseViewModel
    {
        public required string CodeReference { get; set; }
        public required int TarriffCategoriesId { get; set; }

        //public required ViewModelTarriffCategories TarrffCategories {get; set;}

        public required decimal UnitWeight { get; set; }
        public  decimal? Large {  get; set; }
        public  decimal? Broad { get; set; }
        public decimal? High { get; set; }
        
        /*
            public required int CountryId {get; set;}
            public required ViewModelCountrys Countrys {get; set;}
            public required Unit unit {get; set;}
            
         */
    }
}
