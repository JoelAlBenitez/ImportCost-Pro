using Persistence.Context;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationOrderRepository : IImportationOrderRepository
    {
        // El campo _context solo puede asignarse dentro del constructor
        private readonly ContextImportCost _context;

        // Este es el Constructor correcto (se llama igual que la clase, sin 'void', ni 'Task')
        public ImportationOrderRepository(ContextImportCost context)
        {
            _context = context; // Aquí se elimina el error de campo NULL o de solo lectura
        }

        // Aquí implementas los métodos que exige la interfaz
        public async Task<ImportationOrder?> GetByIdAsync(string orderId)
        {
            return await _context.ImportationOrders.FindAsync(orderId);
        }

        public async Task<IEnumerable<ImportationOrder>> GetAllAsync()
        {
            return await _context.ImportationOrders.ToListAsync();
        }

        public async Task AddAsync(ImportationOrder order)
        {
            await _context.ImportationOrders.AddAsync(order);
        }

        public Task UpdateAsync(ImportationOrder order)
        {
            _context.ImportationOrders.Update(order);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string orderId)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                _context.ImportationOrders.Remove(order);
            }
        }
    }
}