using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Interfaces.Repositories.ImportationOrderAndLandCost
{
    public interface IImportationOrderRepository
    {
        Task<IReadOnlyCollection<ImportationOrder>> GetAllAsync();

        Task<bool> CreateAsync(ImportationOrder order);

        Task<bool> EditAsync(ImportationOrder order);

        Task<ImportationOrder?> GetEntityById(string id);
        Task<bool> DeleteAsync(ImportationOrder order);
    }
}