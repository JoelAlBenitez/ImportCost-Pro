using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationOrderRepository
    {
        private readonly ContextImportCost _context;

        public ImportationOrderRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> HasOrdersByCurrencyId(int currencyId)
        {
            return await _context.ImportationOrders.AnyAsync(o => o.CurrencyId == currencyId);
        }
    }
}