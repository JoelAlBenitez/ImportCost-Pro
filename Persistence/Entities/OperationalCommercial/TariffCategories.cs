using Persistence.Entities.Base;

namespace Persistence.Entities.OperationalCommercial
{
    public class TariffCategories : BaseEntity<int, string>
    {
        public required decimal PorcentageTariff { get; set; }
        public required bool ITBIS {  get; set; }
        public required bool SelectiveTaxApplies { get; set; }
        public  decimal? PorcentageTaxSelective {  get; set; }
        public required string TariffCode { get; set; }
        public ICollection<Products>? Products { get; set; }
    }
}
