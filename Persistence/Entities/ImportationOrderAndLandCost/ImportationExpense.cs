using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.ImportationOrderAndLandCost
{
    public class ImportationExpense
    {
        public required string ImportationExpenseId { get; set; }

        public required string OrderId { get; set; }

        public required string ExpenseType { get; set; }

        public required decimal ExpenseAmount { get; set; }

        public required string CurrencyId { get; set; }

        public required string DistributionMethod { get; set; }

        public required DateTime ImportationExpenseDate { get; set; }
    }
}
