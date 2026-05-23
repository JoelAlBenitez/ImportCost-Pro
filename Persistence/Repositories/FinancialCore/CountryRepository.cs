using Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Entities.FinancialCore;
namespace Persistence.Repositories.FinancialCore
{
    public class CountryRepository : BaseRepository<Country, int>
    {
        public Task<bool> CreateAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Country>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Country> GetEntityById(int key)
        {
            throw new NotImplementedException();
        }
    }
}
