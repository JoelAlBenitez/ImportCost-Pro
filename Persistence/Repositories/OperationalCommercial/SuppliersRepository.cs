using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.OperationalCommercial
{
    public class SuppliersRepository : BaseRepository<Suppliers, int>
    {

        private readonly ContextImportCost _context;

        public SuppliersRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Suppliers entity)
        {
            await _context.Suppliers.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int tkey)
        {
            var entity = await _context.Suppliers.FindAsync(tkey);
            if (entity != null)
            {
                _context.Suppliers.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }
            return false;
        }

        public async Task<bool> EditAsync(Suppliers entity)
        {
            if (entity != null)
            {

                _context.Suppliers.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }
            return false;
        }

        public async Task<IReadOnlyCollection<Suppliers>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .Include(s => s.MainCurrency)
                .Include(s => s.Country)
                .ToListAsync();
        }

        public async Task<Suppliers> GetEntityById(int key)
        {
            return await _context.Suppliers
                .AsNoTracking()
                 .Include(s => s.MainCurrency)
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task<bool> ExistName(string name)
        {
            var s = await _context.Suppliers
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Name.Trim() == name.Trim());
            return s != null;
        }

        public async Task<bool> HasSuppliersByCountryId(int countryId)
        {
            return await _context.Suppliers.AnyAsync(s => s.countryId == countryId);
        }
        public async Task<bool> AssociateImportationOrderBySupplier(int key)
        {
            return await _context.ImportationOrders.AnyAsync(s => s.SupplierId == key);
        }
    }
}