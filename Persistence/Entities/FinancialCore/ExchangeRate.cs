using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.FinancialCore
{
    public class ExchangeRate
    {
        public int SourceCurrencyId { get; set; }
        public int DestinationCurrencyId { get; set; }
        public decimal RateValue { get; set; }
        public DateTime EffectiveDate { get; set; }



        public virtual Currency SourceCurrency { get; set; }
        public virtual Currency DestinationCurrency { get; set; }
    }
}
