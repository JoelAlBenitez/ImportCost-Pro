

using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationExpense
    {
        public required string ImportationExpenseId { get; set; }

        public required string OrderId { get; set; }

        public required ExpenseType ExpenseType { get; set; }

        public required decimal ExpenseAmount { get; set; }

        public required int CurrencyId { get; set; }

        public required DistributionMethod DistributionMethod { get; set; }

        public required DateTime ImportationExpenseDate { get; set; }

        //navigation properties

        public required ImportationOrder ImportationOrder { get; set; }

        public required Currency Currency { get; set; }
    }
}
