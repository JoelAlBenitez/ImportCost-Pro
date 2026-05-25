using Application.ViewModel.Base;
using Persistence.Entities.Enums;
using Application.ViewModel.TarriffCategories;

namespace Application.ViewModel.Products
{
    public class ViewModelProducts : BaseViewModel <int, string>
    {
        public required string CodeReference { get; set; }
        public required int TarriffCategoriesId { get; set; }
        public required ViewModelTarriffCategories  TarrffCategories {get; set;}
        public required decimal UnitWeight { get; set; }
        public  decimal? Large {  get; set; }
        public  decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public UnitMesaurement unitMesaurement { get; set; }
        public required int CountryId {get; set;}  
        
        //public required ViewModelCountrys Countrys {get; set;}
           
      
    }
}
