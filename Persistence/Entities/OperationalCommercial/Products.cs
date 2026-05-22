using Persistence.Entities.Base;
using Application.Enum;
namespace Persistence.Entities.OperationalCommercial
{
    public class Products : BaseEntity<int, string>
    {
        public required string CodeRefence { get; set; }
        public required decimal UnitWeight {  get; set; }
        public decimal? Large { get; set; }
        public decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public string? Description { get; set; }
        public UnitMeasurement Unit { get; set; } 
        public int? tarrifCategoriesId { get; set; }
        public TariffCategories? tariffCategories {get; set;}

        //public Countrys? country {get; set;}
        //public int? countryId {get; set;}
    }
}
