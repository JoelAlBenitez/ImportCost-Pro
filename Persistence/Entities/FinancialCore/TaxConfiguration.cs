using Persistence.Entities.Base;

namespace Persistence.Entities.FinancialCore
{
    public class TaxConfiguration : BaseEntity<int, string>
    {
        public required decimal GeneralItbisPercentage { get; set; }
        public required decimal CustomsServiceRatePercentage { get; set; }
    }
}
