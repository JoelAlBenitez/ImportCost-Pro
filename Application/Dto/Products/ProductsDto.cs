using Application.Dto.Base;
namespace Application.Dto.Products
{
    public  class ProductsDto : DtoBase
    {
        public required string CodeReference { get; set; }
        public required int TarriffCategoriesId {  get; set; }
        //public required TarriffCategoriesDto TarriffCategories {get; set;}
        public required decimal UnitWeight { get; set; }
        public decimal? Large {  get; set; }
        public decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public string? Description { get; set; }

        //public Unit unit {get; set;}
        //public required int CountrysId {get; set;}
    }
}
