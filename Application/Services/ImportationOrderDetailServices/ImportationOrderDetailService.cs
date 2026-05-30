using Application.DTOs.OrderDetails;
using Application.DTOs.Orders;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.ImportationOrderAndLandCost; 
using Persistence.Repositories.OperationalCommercial;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Result;

namespace Application.Services.ImportationOrderDetailServices
{
    public class ImportationOrderDetailService
    {

        private readonly ImportationOrderRepository _orderRepository;
        private readonly OrderDetailRepository _detailRepository;

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


        public async Task<ServiceResult> AddProductToOrderAsync(string orderId, OrderDetailCreateDTO dto)
        {
            var order = await _orderRepository.GetEntityById(orderId);

            if (order == null)
                return new ServiceResult { Success = false, Message = "La orden no existe.", TypeAlert = "error" };

            if (order.OrderState != OrderState.Abierta)
                return new ServiceResult { Success = false, Message = "Solo se pueden agregar productos a órdenes en estado Abierta.", TypeAlert = "warning" };

            if (order.ImportationOrderDetails != null && order.ImportationOrderDetails.Any(d => d.ProductId == dto.ProductId))
                return new ServiceResult { Success = false, Message = "Este producto ya fue agregado a la orden. Edite el registro existente.", TypeAlert = "warning" };

            if (dto.Quantity <= 0)
                return new ServiceResult { Success = false, Message = "La cantidad debe ser mayor a cero.", TypeAlert = "warning" };

            if (dto.FOBUnitPrice <= 0)
                return new ServiceResult { Success = false, Message = "El precio FOB debe ser mayor a cero.", TypeAlert = "warning" };

            if (dto.ExpectedProfitMargin < 0 || dto.ExpectedProfitMargin >= 100)
                return new ServiceResult { Success = false, Message = "El margen de ganancia debe ser mayor o igual a 0 y menor que 100.", TypeAlert = "warning" };
            var newDetail = new ImportationOrderDetail
            {
                OrderDetailId = Guid.NewGuid().ToString(),
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                FOBUnitPrice = dto.FOBUnitPrice,
                ExpectedProfitMargin = dto.ExpectedProfitMargin
            };

            await _detailRepository.AddAsync(newDetail);

            // Retornamos éxito en lugar de "true"
            return new ServiceResult { Success = true, Message = "Producto agregado correctamente.", TypeAlert = "success" }; 
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

            await _detailRepository.UpdateAsync(detail); 
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