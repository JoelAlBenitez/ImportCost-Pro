using Persistence.Entities.Base;

namespace Persistence.Entities.OperationalCommercial
{
    public class TariffCategories : BaseEntity<string, string>
    {
        public required decimal PorcentageTariff { get; set; }
        public required bool ITBIS {  get; set; }
        public required bool SelectiveTaxApplies { get; set; }
        public  decimal? PorcentageTaxSelective {  get; set; }
        public ICollection<Products>? Products { get; set; }
        public string TariffCode { get; internal set; }
    }
}
