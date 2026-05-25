using Persistence.Entities.Base;
using Persistence.Entities.FinancialCore;

namespace Persistence.Entities.OperationalCommercial
{
    public class Suppliers : BaseEntity<int, string>
    {
        public  Country? Country {get;set;}
        public required int countryId {get; set;}
      
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Currency? MainCurrency {get; set;}
        public int? MainCurrencyId { get; set;  }
    }
}
