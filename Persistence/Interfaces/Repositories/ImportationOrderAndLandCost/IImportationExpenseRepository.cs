using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Interfaces.Repositories.ImportationOrderAndLandCost
{
    public interface IImportationExpenseRepository
    {
        Task<IReadOnlyCollection<ImportationExpense>> GetByOrderIdAsync(string orderId);
    }
}