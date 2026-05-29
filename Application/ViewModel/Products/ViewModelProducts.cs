using Application.ViewModel.Base;
using Persistence.Entities.Enums;

namespace Application.ViewModel.Products
{
    public class ViewModelProducts : BaseViewModel <int, string>
    {
        public required string CodeReference { get; set; }
        public required string TariffCategoriesId { get; set; }
        public required string TarffCategoriesName { get; set; }
        public required decimal UnitWeight { get; set; }
        public  decimal? Large {  get; set; }
        public  decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public UnitMesaurement unitMesaurement { get; set; }
        public required int CountryId {get; set;}  
        public required string CountryName { get; set; }
  
    }
}
