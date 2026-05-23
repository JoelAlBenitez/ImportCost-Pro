using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.Base;


namespace Persistence.Repositories.OperationalCommercial
{
    public class TariffCategoriesRepository : BaseRepository<TariffCategories, int>
    {
        private readonly ContextImportCost _context;

        public TariffCategoriesRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TariffCategories entity)
        {
            await _context.tariffCategories.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int tkey)
        {
            var entity = await _context.tariffCategories.FindAsync(tkey);
            if(entity != null)
            {
               _context.tariffCategories.Remove(entity);
               var result= await _context.SaveChangesAsync();
               return result > 0;

            }return false;
        }

        public async Task<bool> EditAsync(TariffCategories entity)
        {
            if(entity != null)
            {
                _context.tariffCategories.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }return false;
        }

        public async Task<IReadOnlyCollection<TariffCategories>> GetAllAsync()
        {
            return await _context.tariffCategories.Where(t => t.State == true)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TariffCategories> GetEntityById(int key)
        {
           return await _context.tariffCategories
                .AsNoTracking()
                .FirstAsync(t =>  t.Key == key);
        }

        public async Task<bool> ExistTariffCode(string code)
        {
            return await _context.tariffCategories
                .AsNoTracking()
                .FirstAsync(t => t.TariffCode == code) != null;
        }
    }
}
