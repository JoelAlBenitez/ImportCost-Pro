using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities.FinancialCore;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationOrderRepository : IImportationOrderRepository
    {
        private readonly ContextImportCost _context;

        public ImportationOrderRepository(ContextImportCost context)
        {
            _context = context;
        }

        // 1. GET BY ID
        public async Task<ImportationOrder?> GetEntityById(string orderId)
        {
            return await _context.ImportationOrders
        .Include(o => o.Importer)
        .Include(o => o.Supplier)
        .Include(o => o.Country)
        .Include(o => o.Currency)
        .Include(o => o.ImportationOrderDetails)
        .Include(o => o.ImportationExpenses)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        // 2. GET ALL
        public async Task<IReadOnlyCollection<ImportationOrder>> GetAllAsync()
        {
            return await _context.ImportationOrders
        .Include(o => o.Importer)
        .Include(o => o.Supplier)
        .Include(o => o.Country)
        .Include(o => o.Currency)
        .Include(o => o.ImportationOrderDetails)
        .Include(o => o.ImportationExpenses)
        .ToListAsync();
        }

        // 3. CREATE
        public async Task<bool> CreateAsync(ImportationOrder order)
        {
            await _context.ImportationOrders.AddAsync(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // 4. EDIT
        public async Task<bool> EditAsync(ImportationOrder order)
        {
            _context.ImportationOrders.Update(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // 5. DELETE
        public async Task<bool> DeleteAsync(ImportationOrder order)
        {
            _context.ImportationOrders.Remove(order);
            var result = await _context.SaveChangesAsync(); 
            return result > 0;
        }
    }
}