using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    // 1. Queda como clase independiente
    public class ImportationExpenseRepository
    {
        private readonly ContextImportCost _context;

        public ImportationExpenseRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ImportationExpense>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationExpenses
                                 .AsNoTracking() // 2. Regla de Joel
                                 .Where(e => e.OrderId == orderId)
                                 .ToListAsync();
        }
    }
}