using Application.DTOs.Base;
namespace Application.DTOs.TarriffCategories
{
    public class TariffCategoriesDto : DtoBase<string>
    {
        public required decimal PorcentageTariff { get; set; }
        public required bool ITBIS { get; set; }
        public required bool SelectiveTaxApplies { get; set; }
        public decimal? PorcentageTaxSelective { get; set; }
    }
}
