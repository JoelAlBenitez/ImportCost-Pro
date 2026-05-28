using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.FinancialCore
{
    public class CurrencyRepository : BaseRepository<Currency, int>
    {
        private readonly ContextImportCost _context;

        public CurrencyRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Currency entity)
        {
            await _context.currencies.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(Currency entity)
        {
            _context.currencies.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.currencies.FindAsync(key);
            if (entity != null)
            {
                _context.currencies.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Currency> GetEntityById(int key)
        {
            return await _context.currencies.FirstAsync(c => c.Key == key);
        }

        public async Task<IReadOnlyCollection<Currency>> GetAllAsync()
        {
            return await _context.currencies.ToListAsync();
        }

        public async Task<Currency?> GetByIsoCodeAsync(string isoCode)
        {
            return await _context.currencies
                .FirstOrDefaultAsync(c => c.IsoCode.ToLower() == isoCode.ToLower());
        }

        public async Task<Currency?> GetLocalCurrencyAsync()
        {
            return await _context.currencies
                .FirstOrDefaultAsync(c => c.IsLocalCurrency == true && c.State == true);
        }
    }
}
