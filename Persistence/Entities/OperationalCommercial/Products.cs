using Persistence.Entities.Base;
using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;
namespace Persistence.Entities.OperationalCommercial
{
    public class Products : BaseEntity<int, string>
    {
        public required string CodeRefence { get; set; }
        public required decimal UnitWeight { get; set; }
        public decimal? Large { get; set; }
        public decimal? Broad { get; set; }
        public decimal? High { get; set; }
        public string? Description { get; set; }
        public required UnitMesaurement Unit { get; set; }
        public required int tarrifCategoriesId { get; set; }
        public TariffCategories? tariffCategories { get; set; }
        public Country? country { get; set; }
        public required int countryId { get; set; }
    }
}