using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.OperationalCommercial
{
    public class ProductsRepository : BaseRepository<Products, int>
    {
        private readonly ContextImportCost _context;

        public ProductsRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Products entity)
        {
            await _context.products.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> DeleteAsync(int tkey)
        {
            var entity = await _context.products.FindAsync(tkey);
            if (entity != null)
            {
                _context.products.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> EditAsync(Products entity)
        {
            if (entity != null)
            {
                _context.products.Update(entity);
                return await _context.SaveChangesAsync() > 0;

            }
            return false;

        }

        public async Task<IReadOnlyCollection<Products>> GetAllAsync()
        {
            return await _context.products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Products> GetEntityById(int key)
        {
            return await _context.products
                .AsNoTracking()
                .FirstAsync(p => p.Key == key);
        }

        public async Task<bool> ExistProductsByCodeReference(string codeReference)
        {
            return await _context.products.FirstAsync(p => p.CodeRefence == codeReference) != null;        }

        public async Task<bool> HasProductsByCountryId(int countryId)
        {
            return await _context.products.AnyAsync(p => p.countryId == countryId);
        }
    }
}
