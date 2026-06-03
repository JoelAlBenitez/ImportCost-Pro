using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.FinancialCore
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate, int>
    {
        private readonly ContextImportCost _context;

        public ExchangeRateRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(ExchangeRate entity)
        {
            await _context.ExchangeRates.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(ExchangeRate entity)
        {
            _context.ExchangeRates.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.ExchangeRates.FindAsync(key);
            if (entity != null)
            {
                _context.ExchangeRates.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<ExchangeRate> GetEntityById(int key)
        {
            return (await _context.ExchangeRates
                .AsNoTracking()
                .Include(e => e.SourceCurrency)
                .Include(e => e.DestinationCurrency)
                .FirstOrDefaultAsync(e => e.Key == key))!;
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetAllAsync()
        {
            return await _context.ExchangeRates
                .AsNoTracking()
                .Include(e => e.SourceCurrency)
                .Include(e => e.DestinationCurrency)
                .ToListAsync();
        }

        public async Task<bool> HasExchangeRatesByCurrencyId(int currencyId)
        {
            return await _context.ExchangeRates
                .AsNoTracking()
                .AnyAsync(e => e.SourceCurrencyId == currencyId || e.DestinationCurrencyId == currencyId);
        }

        public async Task<ExchangeRate?> GetLatestRateAsync(int sourceId, int destinationId, DateTime date)
        {
            return await _context.ExchangeRates
                .AsNoTracking()
                .Where(e => e.SourceCurrencyId == sourceId 
                         && e.DestinationCurrencyId == destinationId 
                         && e.State == true 
                         && e.EffectiveDate <= date)
                .OrderByDescending(e => e.EffectiveDate)
                .FirstOrDefaultAsync();
        }
    }
}
