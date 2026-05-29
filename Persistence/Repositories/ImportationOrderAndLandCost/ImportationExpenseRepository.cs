using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationExpenseRepository : IImportationExpenseRepository
    {
        private readonly ContextImportCost _context;

        public ImportationExpenseRepository(ContextImportCost context)
        {
            _context = context;
        }

        // 1. Cambiamos el retorno a IReadOnlyCollection
        public async Task<IReadOnlyCollection<ImportationExpense>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationExpenses
                                 .AsNoTracking()
                                 .Where(e => e.OrderId == orderId)
                                 .ToListAsync();
        }
    }
}