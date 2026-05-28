using Application.DTOs.Base;
using Persistence.Entities.Enums;
namespace Application.DTOs.Products
{
    public  class ProductsDto : DtoBase<int>
    {
        public required string CodeReference { get; set; }
        public required string TarriffCategoriesId {  get; set; }
        public string? TariffCategoriesName { get; set; }
        public required decimal UnitWeight { get; set; }
        public decimal? Large {  get; set; }
        public decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public string? Description { get; set; }
        public required UnitMesaurement unitMesaurement { get; set; }
        public required int CountrysId {get; set;}
        public string? CountryName { get; set; }

    }
}
