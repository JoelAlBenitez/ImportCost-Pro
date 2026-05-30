using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationExpenseRepository
    {
        private readonly ContextImportCost _context;

        public ImportationExpenseRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> HasExpensesByCurrencyId(int currencyId)
        {
            return await _context.ImportationExpenses.AnyAsync(e => e.CurrencyId == currencyId);
        }
    }
}