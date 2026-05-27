using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ContextImportCost _context;

        public OrderDetailRepository(ContextImportCost context)
        {
            _context = context;
        }

        // Busca todos los productos que pertenezcan a un OrderId específico
        public async Task<IEnumerable<ImportationOrderDetail>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationOrderDetails
                                 .Where(od => od.OrderId == orderId)
                                 .ToListAsync();
        }

        // Busca un solo detalle por su ID único (usando string)
        public async Task<ImportationOrderDetail?> GetByIdAsync(string id)
        {
            return await _context.ImportationOrderDetails.FindAsync(id);
        }

        public async Task AddAsync(ImportationOrderDetail orderDetail)
        {
            await _context.ImportationOrderDetails.AddAsync(orderDetail);
        }

        public Task UpdateAsync(ImportationOrderDetail orderDetail)
        {
            _context.ImportationOrderDetails.Update(orderDetail);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var orderDetail = await GetByIdAsync(id);
            if (orderDetail != null)
            {
                _context.ImportationOrderDetails.Remove(orderDetail);
            }
        }
    }
}