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
            await _context.Products.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> DeleteAsync(int tkey)
        {
            var entity = await _context.Products.FindAsync(tkey);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> EditAsync(Products entity)
        {
            if (entity != null)
            {
                _context.Products.Update(entity);
                return await _context.SaveChangesAsync() > 0;

            }
            return false;

        }

        public async Task<IReadOnlyCollection<Products>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.country)
                .Include(p => p.tariffCategories)
                .ToListAsync();
        }

        public async Task<Products> GetEntityById(int key)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.country)
                .Include(p => p.tariffCategories)
                .FirstOrDefaultAsync(p => p.Key == key);
        }

        public async Task<bool> ExistProductsByCodeReference(string codeReference)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.CodeRefence == codeReference) != null;
        }

        public async Task<bool> HasProductsByCountryId(int countryId)
        {
            return await _context.Products.AnyAsync(p => p.countryId == countryId);
        }
        public async Task<bool> AssociateImportationOrderDetailsByProducts(int key)
        {
            return await _context.ImportationOrderDetails.AnyAsync(p => p.ProductId == key);
        }
    }
}