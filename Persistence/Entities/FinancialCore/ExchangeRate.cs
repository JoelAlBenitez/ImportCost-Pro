using Persistence.Entities.Base;

namespace Persistence.Entities.FinancialCore
{
    public class ExchangeRate : BaseEntity<int, string>
    {
        public int SourceCurrencyId { get; set; }
        public int DestinationCurrencyId { get; set; }
        public decimal RateValue { get; set; }
        public DateTime EffectiveDate { get; set; }



        public Currency? SourceCurrency { get; set; }
        public Currency? DestinationCurrency { get; set; }
    }
}