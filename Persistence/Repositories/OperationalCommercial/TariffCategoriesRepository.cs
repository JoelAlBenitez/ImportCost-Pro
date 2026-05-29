using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.Base;


namespace Persistence.Repositories.OperationalCommercial
{
    public class TariffCategoriesRepository : BaseRepository<TariffCategories, string>
    {
        private readonly ContextImportCost _context;

        public TariffCategoriesRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TariffCategories entity)
        {
            await _context.TariffCategories.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
     
        public async Task<bool> DeleteAsync(string tkey)
        {
            var entity = await _context.TariffCategories.FindAsync(tkey);
            if (entity != null)
            {
               _context.TariffCategories.Remove(entity);
               var result= await _context.SaveChangesAsync();
               return result > 0;

            }
            return false;
        }
        public async Task<bool> EditAsync(TariffCategories entity)
        {
            if(entity != null)
            {
                _context.TariffCategories.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }return false;
        }

        public async Task<bool> AssociatedProductsC(string code)
        {
           
            return await _context.products.AnyAsync(c => c.tarrifCategoriesId == code);
        }

        public async Task<IReadOnlyCollection<TariffCategories>> GetAllAsync()
        {
            return await _context.tariffCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TariffCategories> GetEntityById(string key)
        {
            return await _context.tariffCategories
                 .AsNoTracking()
                 .FirstAsync(t => t.Key == key);
        }

        
    }
}
