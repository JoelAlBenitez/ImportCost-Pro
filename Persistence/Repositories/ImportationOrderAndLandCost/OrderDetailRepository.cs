using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class OrderDetailRepository
    {
        private readonly ContextImportCost _context;

        public OrderDetailRepository(ContextImportCost context)
        {
            _context = context;
        }
        public async Task<IReadOnlyCollection<ImportationOrderDetail>> GetByOrderIdAsync(string orderId)
        {
            return await _context.ImportationOrderDetails
                                 .AsNoTracking()
                                 .Where(od => od.OrderId == orderId)
                                 .ToListAsync();
        }

        public async Task<ImportationOrderDetail?> GetByIdAsync(string orderDetailId)
        {
            return await _context.ImportationOrderDetails.Include(d => d.Product)
            .FirstOrDefaultAsync(d => d.OrderDetailId == orderDetailId);
        }
        public async Task<bool> AddAsync(ImportationOrderDetail orderDetail)
        {
            await _context.ImportationOrderDetails.AddAsync(orderDetail);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(ImportationOrderDetail orderDetail)
        {
            _context.ImportationOrderDetails.Update(orderDetail);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var orderDetail = await GetByIdAsync(id);
            if (orderDetail != null)
            {
                _context.ImportationOrderDetails.Remove(orderDetail);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }
    }
}