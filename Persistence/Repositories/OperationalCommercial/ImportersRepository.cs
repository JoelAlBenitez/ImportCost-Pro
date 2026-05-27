using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.Base;

namespace Persistence.Repositories.OperationalCommercial
{
    public class ImportersRepository : BaseRepository<Importers, int>
    {

        private readonly ContextImportCost _context;

        public ImportersRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Importers entity)
        {
            await _context.Importers.AddAsync(entity);
           var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Importers entity)
        {
            if(entity != null) {

                 _context.Importers.Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> EditAsync(Importers entity)
        {
            if (entity != null) {

                _context.Importers.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0;

            }return false;
        }

        public async Task<IReadOnlyCollection<Importers>> GetAllAsync()
        {
            return await _context.Importers.Where(i => i.State == true).ToListAsync();
        }

        public async Task<Importers> GetEntityById(int key)
        {
            return await _context.Importers.FirstAsync(i => i.Key == key); ;
        }

        public async Task<bool> ExistImportersByRnc(string rnc)
        {
            return await _context.Importers.FirstAsync(i => i.Identification == rnc) != null; 
        }
    }
}
