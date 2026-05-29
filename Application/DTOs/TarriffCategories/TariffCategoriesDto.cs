using Application.DTOs.Base;
namespace Application.DTOs.TarriffCategories
{
    public class TariffCategoriesDto : DtoBase<string>
    {
        public decimal PorcentageTariff { get; set; }
        public bool ITBIS { get; set; }
        public bool SelectiveTaxApplies { get; set; }
        public decimal? PorcentageTaxSelective { get; set; }

    }
}