using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Interfaces.Repositories.ImportationOrderAndLandCost
{
    public interface IImportationOrderRepository
    {
        Task<ImportationOrder?> GetByIdAsync(string orderId);
        Task<IEnumerable<ImportationOrder>> GetAllAsync();
        Task AddAsync(ImportationOrder order);
        Task UpdateAsync(ImportationOrder order);
        Task DeleteAsync(string orderId);
    }
}