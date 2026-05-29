using Application.DTOs.Orders;
// BORRAMOS: using Application.Interfaces.Services;
using Microsoft.Identity.Client;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
// BORRAMOS: using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using Persistence.Repositories.ImportationOrderAndLandCost; // AGREGAMOS EL NAMESPACE DE LAS CLASES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    // 1. Ya no hereda de IImportationOrderService
    public class ImportationOrderService
    {
        // 2. Usamos la CLASE concreta de tu repositorio
        private readonly ImportationOrderRepository _orderRepository;

        public ImportationOrderService(ImportationOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // --- LOS MÉTODOS SE QUEDAN IGUALES POR AHORA ---
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
                TotalFOB = order.ImportationOrderDetails != null
                ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0,
                TotalImportationExpected =
                (order.ImportationOrderDetails != null ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0) +
                (order.ImportationExpenses != null ? order.ImportationExpenses.Sum(e => e.ExpenseAmount) : 0)
            }).ToList();

            return dtos;
        }

        public async Task<ImportationOrderResponseDTO> CreateAsync(ImportationOrderCreateDTO orderCreateDTO)
        {
            var newOrder = new ImportationOrder
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.UtcNow,
                OrderState = OrderState.Abierta,

                ImporterId = orderCreateDTO.ImporterId,
                SupplierId = orderCreateDTO.SupplierId,
                OriginCountryId = orderCreateDTO.OriginCountryId,
                TransportMode = orderCreateDTO.TransportMode,
                CurrencyId = orderCreateDTO.CurrencyId,
            };

            await _orderRepository.CreateAsync(newOrder);

            var response = new ImportationOrderResponseDTO
            {
                OrderId = newOrder.OrderId,
                ImporterId = newOrder.ImporterId,
                SupplierId = newOrder.SupplierId,
                OriginCountryId = newOrder.OriginCountryId,
                CurrencyId = newOrder.CurrencyId,
                OrderDate = newOrder.OrderDate,
                TransportMode = newOrder.TransportMode,
                OrderState = newOrder.OrderState,
                TotalFOB = 0,
                TotalImportationExpected = 0
            };

            return response;
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

                TotalFOB = order.ImportationOrderDetails != null
            ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice)
            : 0,

                TotalImportationExpected =
            (order.ImportationOrderDetails != null ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0) +
            (order.ImportationExpenses != null ? order.ImportationExpenses.Sum(e => e.ExpenseAmount) : 0)
            };

            return response;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = await _orderRepository.GetEntityById(id);
            if (order == null) return false;

            if (order.OrderState == OrderState.Calculada)
            {
                throw new InvalidOperationException("No se puede eliminar esta orden porque ya tiene un cálculo oficial de landed cost.");
            }

            var result = await _orderRepository.DeleteAsync(order);
            return result;
        }

        public async Task<ImportationOrderResponseDTO> EditAsync(string id, ImportationOrderUpdateDTO orderUpdateDTO)
        {
            var existingOrder = await _orderRepository.GetEntityById(id);
            if (existingOrder == null) return null;

            if (existingOrder.OrderState == OrderState.Cerrada || existingOrder.OrderState == OrderState.Cancelada)
            {
                throw new InvalidOperationException("No se puede editar esta orden porque está cerrada o cancelada.");
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

                TotalFOB = existingOrder.ImportationOrderDetails != null
                    ? existingOrder.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice)
                    : 0,

                TotalImportationExpected =
                    (existingOrder.ImportationOrderDetails != null ? existingOrder.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0) +
                    (existingOrder.ImportationExpenses != null ? existingOrder.ImportationExpenses.Sum(e => e.ExpenseAmount) : 0)
            };

            return response;
        }

        public async Task<bool> CloseOrderAsync(string id)
        {
            var order = await _orderRepository.GetEntityById(id);
            if (order == null) return false;

            if (order.OrderState != OrderState.Calculada)
            {
                throw new InvalidOperationException("Solo se pueden cerrar órdenes que estén en estado 'Calculada'.");
            }

            if (order.LandedCostSummary == null)
            {
                throw new InvalidOperationException("No se puede cerrar esta orden porque no tiene un resumen de landed cost asociado.");
            }

            order.OrderState = OrderState.Cerrada;
            await _orderRepository.EditAsync(order);
            return true;
        }
    }
}