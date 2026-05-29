using Application.DTOs.OrderDetails;
using Application.DTOs.Orders; // Aquí es donde viven tus OrderDetailCreateDTO y OrderDetailUpdateDTO
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Entities.OperationalCommercial;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Persistence.Repositories.Base; // El namespace de tu interfaz genérica
using Persistence.Repositories.OperationalCommercial;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImportationOrderDetailService
    {
        // Cambiamos a BaseRepository<Entidad, TipoDelID>
        private readonly BaseRepository<ImportationOrder, string> _orderRepository;
        private readonly BaseRepository<ImportationOrderDetail, string> _detailRepository;
        // Asumiendo que el ID de Products es int, según vimos en los DTOs pasados
        private readonly BaseRepository<Products, int> _productRepository;

        public ImportationOrderDetailService(
            BaseRepository<ImportationOrder, string> orderRepository,
            BaseRepository<ImportationOrderDetail, string> detailRepository,
            BaseRepository<Products, int> productRepository)
        {
            _orderRepository = orderRepository;
            _detailRepository = detailRepository;
            _productRepository = productRepository;
        }

        // CAMBIO AQUÍ: Usamos TU OrderDetailCreateDTO
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

            await _detailRepository.CreateAsync(newDetail);
            return true;
        }

        // CAMBIO AQUÍ: Usamos TU OrderDetailUpdateDTO
        public async Task<bool> EditProductInOrderAsync(string orderDetailId, OrderDetailUpdateDTO dto)
        {
            var detail = await _detailRepository.GetEntityById(orderDetailId);
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

            await _detailRepository.EditAsync(detail);
            return true;
        }

        public async Task<bool> RemoveProductFromOrderAsync(string orderDetailId)
        {
            var detail = await _detailRepository.GetEntityById(orderDetailId);
            if (detail == null) return false;

            var order = await _orderRepository.GetEntityById(detail.OrderId);
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("No se puede eliminar este producto porque la orden ya no está Abierta.");

            await _detailRepository.DeleteAsync(orderDetailId);
            return true;
        }
    }
}   