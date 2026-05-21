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
             await  _context.suppliers.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Suppliers entity)
        {
            if (entity != null)
            {
                _context.suppliers.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }return false;
        }

        public async Task<bool> EditAsync(Suppliers entity)
        {
            if (entity != null) {

                _context.suppliers.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            
            }    return false;
        }

        public async Task<IReadOnlyCollection<Suppliers>> GetAllAsync()
        {
            return await _context.suppliers.Where(s => s.State == true).ToListAsync();
        }

        public async Task<Suppliers> GetEntityById(int key)
        {
            return await _context.suppliers.FirstAsync(s => s.Key == key); 
        }


    }
}
