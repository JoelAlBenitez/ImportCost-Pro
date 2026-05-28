using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Interfaces.Repositories.ImportationOrderAndLandCost
{
    public interface IOrderDetailRepository
    {
        // Método clave: Trae todos los productos de una orden específica
        Task<IEnumerable<ImportationOrderDetail>> GetByOrderIdAsync(string orderId);

        // Fíjate que cambiamos 'int id' por 'string id'
        Task<ImportationOrderDetail?> GetByIdAsync(string id);
        Task AddAsync(ImportationOrderDetail orderDetail);
        Task UpdateAsync(ImportationOrderDetail orderDetail);
        Task DeleteAsync(string id);
    }
}