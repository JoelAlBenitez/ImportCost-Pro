using Persistence.Entities.Base;

namespace Persistence.Entities.OperationalCommercial
{
    public class Suppliers : BaseEntity<int, string>
    {
        //public  Countrys Country {get;set;}
        
        //public int? countryId {get; set;}

        public string? Email { get; set; }
        public string? Phone { get; set; }

        //public MainCurrencys MainCurrency {get; set;}
        public int? MainCurrencyId { get; set;  }
    }
}
