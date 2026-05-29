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
            await _context.Currencies.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(Currency entity)
        {
            _context.Currencies.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.Currencies.FindAsync(key);
            if (entity != null)
            {
                _context.Currencies.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Currency> GetEntityById(int key)
        {
            return await _context.Currencies.FirstAsync(c => c.Key == key);
        }

        public async Task<IReadOnlyCollection<Currency>> GetAllAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<Currency?> GetByIsoCodeAsync(string isoCode)
        {
            return await _context.Currencies
                .FirstOrDefaultAsync(c => c.IsoCode.ToLower() == isoCode.ToLower());
        }

        public async Task<Currency?> GetLocalCurrencyAsync()
        {
            return await _context.Currencies
                .FirstOrDefaultAsync(c => c.IsLocalCurrency == true && c.State == true);
        }
    }
}
