using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Repositories.Base; 

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    
    public class ImportationExpenseRepository : BaseRepository<ImportationExpense, string>

    {
        private readonly ContextImportCost _context;

        public ImportationExpenseRepository(ContextImportCost context)
        {
            _context = context;
        }



        public async Task<bool> CreateAsync(ImportationExpense entity)
        {
            await _context.ImportationExpenses.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(ImportationExpense entity)
        {
            _context.ImportationExpenses.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await GetEntityById(id);
            if (entity == null) return false;

            _context.ImportationExpenses.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ImportationExpense> GetEntityById(string key)
        {

            return (await _context.ImportationExpenses
                         .FirstOrDefaultAsync(e => e.ImportationExpenseId == key))!;
        }

        public async Task<IReadOnlyCollection<ImportationExpense>> GetAllAsync()
        {
            return await _context.ImportationExpenses.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyCollection<ImportationExpense>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationExpenses
                                 .AsNoTracking()
                                 .Where(e => e.ImportationOrderId == orderId)
                                 .ToListAsync();
        }
        public async Task<bool> HasExpensesByCurrencyId(int currencyId)
        {
            return await _context.ImportationExpenses.AnyAsync(e => e.CurrencyId == currencyId);
        }
    }
}