using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.FinancialCore
{
    public class TaxConfigurationRepository : BaseRepository<TaxConfiguration, int>
    {
        private readonly ContextImportCost _context;

        public TaxConfigurationRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TaxConfiguration entity)
        {
            await _context.TaxConfigurations.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(TaxConfiguration entity)
        {
            _context.TaxConfigurations.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.TaxConfigurations.FindAsync(key);
            if (entity != null)
            {
                _context.TaxConfigurations.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<TaxConfiguration?> GetEntityById(int key)
        {
            return await _context.TaxConfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Key == key);
        }

        public async Task<IReadOnlyCollection<TaxConfiguration>> GetAllAsync()
        {
            return await _context.TaxConfigurations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaxConfiguration?> GetCurrentConfigAsync()
        {
            return await _context.TaxConfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.State == true);
        }
    }
}
