using Application.DTOs.Orders;
using Application.Services.Result;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Repositories.ImportationOrderAndLandCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.ImportationOrderServices
{

    public class ImportationOrderService
    {
        private readonly ImportationOrderRepository _orderRepository;

        public ImportationOrderService(ImportationOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private decimal CalculateTotalFob(ImportationOrder order)
        {
            return  order.ImportationOrderDetails != null
                ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice)
                : 0;
            } 

        private decimal CalculateTotalExpected(ImportationOrder order)
        {
            decimal totalFob = CalculateTotalFob(order);
            decimal totalExpenses = order.ImportationExpenses != null
                ? order.ImportationExpenses.Sum(e => e.ExpenseAmount)
                : 0;

            return totalFob + totalExpenses;
        }

        public async Task<IEnumerable<ImportationOrderResponseDTO>> GetAllAsync()
        {
            var entities = await _orderRepository.GetAllAsync();

            var dtos = entities.Select(order => new ImportationOrderResponseDTO
            {
                OrderId = order.OrderId,
                ImporterId = order.ImporterId,
                SupplierId = order.SupplierId,
                OriginCountryId = order.OriginCountryId,
                CurrencyId = order.CurrencyId,
                OrderDate = order.OrderDate,
                TransportMode = order.TransportMode,
                OrderState = order.OrderState,
                TotalFOB = CalculateTotalFob(order),
                TotalImportationExpected = CalculateTotalExpected(order)
            }).ToList();

            return dtos;
        }

   
        public async Task<ServiceResult> CreateOrderAsync(ImportationOrderCreateDTO dto)
        {
            string finalOrderId = dto.OrderId?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(finalOrderId))
                return new ServiceResult { Success = false, Message = "El número de orden es requerido.", TypeAlert = "warning" };

            if (!finalOrderId.StartsWith("ORIM-", StringComparison.OrdinalIgnoreCase))
            {
                finalOrderId = $"ORIM-{finalOrderId}";
            }

            finalOrderId = finalOrderId.ToUpper();

            var existingOrder = await _orderRepository.GetEntityById(finalOrderId);
            if (existingOrder != null)
            {
                return new ServiceResult { Success = false, Message = $"Ya existe una orden de importación registrada con el número {finalOrderId}.", TypeAlert = "warning" };
            }

            var newOrder = new ImportationOrder
            {
                OrderId = finalOrderId,
                ImporterId = dto.ImporterId,
                SupplierId = dto.SupplierId,
                OriginCountryId = dto.OriginCountryId,
                CurrencyId = dto.CurrencyId,
                TransportMode = dto.TransportMode,
                OrderDate = dto.OrderDate,
                OrderState = OrderState.Abierta,

            };

            bool saved = await _orderRepository.CreateAsync(newOrder);

            if (!saved)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "No se pudo registrar la orden de importación en la base de datos. Intente nuevamente.",
                    TypeAlert = "danger"
                };
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Orden de importación creada exitosamente.",
                TypeAlert = "success"
            };
        }

        public async Task<ImportationOrderResponseDTO> GetEntityById(string id)
        {
            var order = await _orderRepository.GetEntityById(id);
            if (order == null) return null;

            var response = new ImportationOrderResponseDTO
            {
                OrderId = order.OrderId,
                ImporterId = order.ImporterId,
                SupplierId = order.SupplierId,
                OriginCountryId = order.OriginCountryId,
                CurrencyId = order.CurrencyId,
                ImporterName = order.Importer?.Name,
                SupplierName = order.Supplier?.Name,
                OriginCountryName = order.Country?.Name,
                CurrencyCode = order.Currency?.IsoCode,
                OrderDate = order.OrderDate,
                TransportMode = order.TransportMode,
                OrderState = order.OrderState,

                TotalFOB = CalculateTotalFob(order),
                TotalImportationExpected = CalculateTotalExpected(order)
            };

            return response;
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            var order = await _orderRepository.GetEntityById(id);
            if (order == null)
            {
                return new ServiceResult { Success = false, Message = "La orden no fue encontrada.", TypeAlert = "danger" };
            }

            if (order.OrderState == OrderState.Calculada ||
                order.OrderState == OrderState.Cerrada ||
                order.OrderState == OrderState.Cancelada)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"No se puede eliminar esta orden porque se encuentra en estado '{order.OrderState}'.",
                    TypeAlert = "warning"
                };
            }

            await _orderRepository.DeleteAsync(order);

            return new ServiceResult
            {
                Success = true,
                Message = "La orden fue eliminada correctamente.",
                TypeAlert = "success"
            };



        }

        public async Task<ServiceResult> EditAsync(string id, ImportationOrderUpdateDTO orderUpdateDTO)
        {
            var existingOrder = await _orderRepository.GetEntityById(id);
            if (existingOrder == null)
            {
                return new ServiceResult { Success = false, Message = "La orden de importación no fue encontrada.", TypeAlert = "danger" };
            }

            if (existingOrder.OrderState == OrderState.Cerrada || existingOrder.OrderState == OrderState.Cancelada)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "No se puede editar esta orden porque está cerrada o cancelada.",
                    TypeAlert = "warning"
                };

            }
                existingOrder.ImporterId = orderUpdateDTO.ImporterId;
                existingOrder.SupplierId = orderUpdateDTO.SupplierId;
                existingOrder.OriginCountryId = orderUpdateDTO.OriginCountryId;
                existingOrder.CurrencyId = orderUpdateDTO.CurrencyId;
                existingOrder.TransportMode = orderUpdateDTO.TransportMode;

                await _orderRepository.EditAsync(existingOrder);

                var response = new ImportationOrderResponseDTO
                {
                    OrderId = existingOrder.OrderId,
                    ImporterId = existingOrder.ImporterId,
                    SupplierId = existingOrder.SupplierId,
                    OriginCountryId = existingOrder.OriginCountryId,
                    CurrencyId = existingOrder.CurrencyId,
                    OrderDate = existingOrder.OrderDate,
                    TransportMode = existingOrder.TransportMode,
                    OrderState = existingOrder.OrderState,
                    TotalFOB = CalculateTotalFob(existingOrder),
                    TotalImportationExpected = CalculateTotalExpected(existingOrder)
                };

                return new ServiceResult
                {
                    Success = true,
                    Message = "La orden de importación fue editada correctamente.",
                    TypeAlert = "success"
                };
            }

        public async Task<ServiceResult> CloseOrderAsync(string id)
        {
            var order = await _orderRepository.GetEntityById(id);
            if (order == null)
            {
                return new ServiceResult { Success = false, Message = "La orden de importación no fue encontrada.", TypeAlert = "danger" };
            }

            if (order.OrderState != OrderState.Calculada)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Solo se pueden cerrar órdenes que estén en estado 'Calculada'.",
                    TypeAlert = "warning"
                };
            }

            if (order.LandedCostSummary == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "No se puede cerrar esta orden porque no tiene un resumen de landed cost asociado.",
                    TypeAlert = "warning"
                };
            }

            order.OrderState = OrderState.Cerrada;
            await _orderRepository.EditAsync(order);
            return new ServiceResult
            {
                Success = true,
                Message = "La orden ha sido cerrada exitosamente. Ya no podrá ser modificada.",
                TypeAlert = "success"
            };
        }
    }
}