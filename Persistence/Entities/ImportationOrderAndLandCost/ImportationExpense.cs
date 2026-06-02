

using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationExpense
    {
        public required string ImportationExpenseId { get; set; }

        public required string ImportationOrderId { get; set; }

        public required ExpenseType ExpenseType { get; set; }

        public required decimal ExpenseAmount { get; set; }

        public required int CurrencyId { get; set; }

        public required DistributionMethod DistributionMethod { get; set; }

        public required DateTime ImportationExpenseDate { get; set; }

        //navigation properties

        public ImportationOrder? ImportationOrder { get; set; }

        public Currency? Currency { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
