using Persistence.Entities.Base;
using Persistence.Entities.OperationalCommercial;
using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Entities.FinancialCore
{
    public class Currency : BaseEntity<int, string> 
    {
        public required string IsoCode { get; set; }
        public required string Symbol { get; set; }
        public required bool IsLocalCurrency { get; set; }

     
        public ICollection<Suppliers>? Suppliers { get; set; }
        public ICollection<ExchangeRate>? ExchangeRatesSource { get; set; }
        public ICollection<ExchangeRate>? ExchangeRatesDestination { get; set; }
        public ICollection<ImportationOrder>? ImportationOrders { get; set; }
        public ICollection<ImportationExpense>? ImportationExpenses { get; set; }
    

    }
}
