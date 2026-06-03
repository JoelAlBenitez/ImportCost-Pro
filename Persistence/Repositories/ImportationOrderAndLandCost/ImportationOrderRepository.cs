using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationOrderRepository
    {
        private readonly ContextImportCost _context;

        public ImportationOrderRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<ImportationOrder?> GetEntityById(string orderId)
        {
            return await _context.ImportationOrders
                .Include(o => o.Importer)
                .Include(o => o.Supplier)
                .Include(o => o.Country)
                .Include(o => o.Currency)
                .Include(o => o.ImportationOrderDetails)
                .ThenInclude(d => d.Product)         
                .Include(o => o.ImportationExpenses)
                .ThenInclude(e => e.Currency)
                .Include(o => o.LandedCostSummary)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IReadOnlyCollection<ImportationOrder>> GetAllAsync()
        {
            return await _context.ImportationOrders
                .AsNoTracking()
                .Include(o => o.Importer)
                .Include(o => o.Supplier)
                .Include(o => o.Country)
                .Include(o => o.Currency)
                .Include(o => o.ImportationOrderDetails)
                .Include(o => o.ImportationExpenses)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(ImportationOrder order)
        {
            await _context.ImportationOrders.AddAsync(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditAsync(ImportationOrder order)
        {
            _context.ImportationOrders.Update(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(ImportationOrder order)
        {
            _context.ImportationOrders.Remove(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}