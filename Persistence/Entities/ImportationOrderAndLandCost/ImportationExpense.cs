

using Persistence.Entities.Enums;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationExpense
    {
        public required string ImportationExpenseId { get; set; }

        public required string OrderId { get; set; }

        public required ExpenseType ExpenseType { get; set; }

        public required decimal ExpenseAmount { get; set; }

        public required string CurrencyId { get; set; }

        public required string DistributionMethod { get; set; }

        public required DateTime ImportationExpenseDate { get; set; }

        //navigation properties

        public virtual ImportationOrder ImportationOrder { get; set; } = null!;

        //public required Currencies Currencies { get; set; }
    }
}
