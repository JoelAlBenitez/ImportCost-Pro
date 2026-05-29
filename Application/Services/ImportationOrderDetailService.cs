using Application.DTOs.OrderDetails;
using Application.DTOs.Orders;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Entities.OperationalCommercial;
// BORRAMOS: using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
// BORRAMOS: using Persistence.Repositories.Base; 
using Persistence.Repositories.ImportationOrderAndLandCost; // AGREGAMOS
using Persistence.Repositories.OperationalCommercial; // AGREGAMOS (Para ProductsRepository)
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImportationOrderDetailService
    {
        // 1. Usamos las Clases concretas (sin la 'I' y sin BaseRepository)
        private readonly ImportationOrderRepository _orderRepository;
        private readonly OrderDetailRepository _detailRepository;

        // Asumimos que tu compañero creó esta clase sin la 'I'
        private readonly ProductsRepository _productRepository;

        public ImportationOrderDetailService(
            ImportationOrderRepository orderRepository,
            OrderDetailRepository detailRepository,
            ProductsRepository productRepository)
        {
            _orderRepository = orderRepository;
            _detailRepository = detailRepository;
            _productRepository = productRepository;
        }

        // --- LOS MÉTODOS SE QUEDAN IGUALES POR AHORA ---
        public async Task<bool> AddProductToOrderAsync(string orderId, OrderDetailCreateDTO dto)
        {
            var order = await _orderRepository.GetEntityById(orderId);
            if (order == null) throw new Exception("La orden no existe.");
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("Solo se pueden agregar productos a órdenes en estado Abierta.");

            if (order.ImportationOrderDetails != null && order.ImportationOrderDetails.Any(d => d.ProductId == dto.ProductId))
                throw new InvalidOperationException("Este producto ya fue agregado a la orden. Edite el registro existente.");

            if (dto.Quantity <= 0) throw new InvalidOperationException("La cantidad debe ser mayor a cero.");
            if (dto.FOBUnitPrice <= 0) throw new InvalidOperationException("El precio FOB debe ser mayor a cero.");
            if (dto.ExpectedProfitMargin < 0 || dto.ExpectedProfitMargin >= 100)
                throw new InvalidOperationException("El margen de ganancia debe ser mayor o igual a 0 y menor que 100.");

            var newDetail = new ImportationOrderDetail
            {
                OrderDetailId = Guid.NewGuid().ToString(),
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                FOBUnitPrice = dto.FOBUnitPrice,
                ExpectedProfitMargin = dto.ExpectedProfitMargin
            };

            await _detailRepository.AddAsync(newDetail); // NOTA: Cambié CreateAsync por AddAsync según tu repositorio
            return true;
        }

        public async Task<bool> EditProductInOrderAsync(string orderDetailId, OrderDetailUpdateDTO dto)
        {
            var detail = await _detailRepository.GetByIdAsync(orderDetailId);
            if (detail == null) throw new Exception("El detalle del producto no existe.");

            var order = await _orderRepository.GetEntityById(detail.OrderId);
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("No se puede editar este producto porque la orden ya no está Abierta.");

            if (dto.Quantity <= 0) throw new InvalidOperationException("La cantidad debe ser mayor a cero.");
            if (dto.FOBUnitPrice <= 0) throw new InvalidOperationException("El precio FOB debe ser mayor a cero.");
            if (dto.ExpectedProfitMargin < 0 || dto.ExpectedProfitMargin >= 100)
                throw new InvalidOperationException("El margen de ganancia debe ser mayor o igual a 0 y menor que 100.");

            detail.Quantity = dto.Quantity;
            detail.FOBUnitPrice = dto.FOBUnitPrice;
            detail.ExpectedProfitMargin = dto.ExpectedProfitMargin;

            await _detailRepository.UpdateAsync(detail); // NOTA: Cambié EditAsync por UpdateAsync según tu repositorio
            return true;
        }

        public async Task<bool> RemoveProductFromOrderAsync(string orderDetailId)
        {
            var detail = await _detailRepository.GetByIdAsync(orderDetailId);
            if (detail == null) return false;

            var order = await _orderRepository.GetEntityById(detail.OrderId);
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("No se puede eliminar este producto porque la orden ya no está Abierta.");

            await _detailRepository.DeleteAsync(orderDetailId);
            return true;
        }
    }
}