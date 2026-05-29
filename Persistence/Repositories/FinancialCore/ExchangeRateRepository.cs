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
            await _context.exchangeRates.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(ExchangeRate entity)
        {
            _context.exchangeRates.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.exchangeRates.FindAsync(key);
            if (entity != null)
            {
                _context.exchangeRates.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<ExchangeRate> GetEntityById(int key)
        {
            return await _context.exchangeRates
                .Include(e => e.SourceCurrency)
                .Include(e => e.DestinationCurrency)
                .FirstAsync(e => e.Key == key);
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetAllAsync()
        {
            return await _context.exchangeRates
                .Include(e => e.SourceCurrency)
                .Include(e => e.DestinationCurrency)
                .ToListAsync();
        }

        public async Task<ExchangeRate?> GetLatestRateAsync(int sourceId, int destinationId, DateTime date)
        {
            return await _context.exchangeRates
                .Where(e => e.SourceCurrencyId == sourceId 
                         && e.DestinationCurrencyId == destinationId 
                         && e.State == true 
                         && e.EffectiveDate <= date)
                .OrderByDescending(e => e.EffectiveDate)
                .FirstOrDefaultAsync();
        }
    }
}
