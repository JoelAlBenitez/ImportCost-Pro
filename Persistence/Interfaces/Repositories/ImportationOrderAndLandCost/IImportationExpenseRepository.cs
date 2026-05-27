using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Interfaces.Repositories.ImportationOrderAndLandCost
{
    public interface IImportationExpenseRepository
    {
        // Trae todos los gastos asociados a una orden específica
        Task<IEnumerable<ImportationExpense>> GetByOrderIdAsync(string orderId);

        Task<ImportationExpense?> GetByIdAsync(string id);
        Task AddAsync(ImportationExpense expense);
        Task UpdateAsync(ImportationExpense expense);
        Task DeleteAsync(string id);
    }
}