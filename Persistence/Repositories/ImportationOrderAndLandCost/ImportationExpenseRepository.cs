using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationExpenseRepository : IImportationExpenseRepository
    {
        private readonly ContextImportCost _context;

        public ImportationExpenseRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImportationExpense>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationExpenses
                                 .Where(e => e.OrderId == orderId)
                                 .ToListAsync();
        }

        public async Task<ImportationExpense?> GetByIdAsync(string id)
        {
            return await _context.ImportationExpenses.FindAsync(id);
        }

        public async Task AddAsync(ImportationExpense expense)
        {
            await _context.ImportationExpenses.AddAsync(expense);
        }

        public Task UpdateAsync(ImportationExpense expense)
        {
            _context.ImportationExpenses.Update(expense);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var expense = await GetByIdAsync(id);
            if (expense != null)
            {
                _context.ImportationExpenses.Remove(expense);
            }
        }
    }
}