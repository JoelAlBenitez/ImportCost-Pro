using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.FinancialCore
{
    public class CountryRepository : BaseRepository<Country, int>
    {
        private readonly ContextImportCost _context;

        public CountryRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Country entity)
        {
            await _context.Countries.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(Country entity)
        {
            _context.Countries.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var entity = await _context.Countries.FindAsync(key);
            if (entity != null)
            {
                _context.Countries.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Country> GetEntityById(int key)
        {
            return await _context.Countries.FirstAsync(c => c.Key == key);
        }

        public async Task<IReadOnlyCollection<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetByIsoCodeAsync(string isoCode)
        {
            return await _context.Countries
                .FirstOrDefaultAsync(c => c.IsoCode.ToLower() == isoCode.ToLower());
        }
    }
}
