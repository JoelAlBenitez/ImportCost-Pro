using Application.DTOs.Orders;
using Application.Interfaces.Services;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Interfaces.Repositories.ImportationOrderAndLandCost;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class ImportationOrderService : IImportationOrderService
    {
        private readonly IImportationOrderRepository _orderRepository;

        public ImportationOrderService(IImportationOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        //METODOS
        public async Task<IEnumerable<ImportationOrderResponseDTO>> GetAllAsync()
        {
            // LLamar BDD
            var entities = await _orderRepository.GetAllAsync();

            //Mapeo
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

            // 3. Return lista final
            return dtos;
        }

        public async Task<ImportationOrderResponseDTO> CreateAsync(ImportationOrderCreateDTO orderCreateDTO)
        {
            // 1. Mapear de DTO a Entidad
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

                //orden recién creada no tiene productos ni gastos
                TotalFOB = 0,
                TotalImportationExpected = 0
            };

            return response;
        }

        public async Task<ImportationOrderResponseDTO> GetEntityById(string id)
        {
            
            var order = await _orderRepository.GetEntityById(id);

            
            if (order == null)
            {

                return null;
            }

            // 3. Mapear de Entidad a DTO
            var response = new ImportationOrderResponseDTO
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
            ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice)
            : 0,

                TotalImportationExpected =
            (order.ImportationOrderDetails != null ? order.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0) +
            (order.ImportationExpenses != null ? order.ImportationExpenses.Sum(e => e.ExpenseAmount) : 0)
            };

            // 4. Retornar el resultado
            return response;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            // 1. Buscar la entidad pura en la base de datos usando el ID
            var order = await _orderRepository.GetEntityById(id);

            // 2. Validar que la orden exista y no esté calculada
            if (order == null) return false;

            if (order.OrderState == OrderState.Calculada)
            {
                throw new InvalidOperationException("No se puede eliminar esta orden porque ya tiene un cálculo oficial de landed cost.");
            }

            // 3. Enviar la entidad completa al repositorio para que la elimine de SQL
            var result = await _orderRepository.DeleteAsync(order);
            return result;
        }

        public async Task<ImportationOrderResponseDTO> EditAsync(string id, ImportationOrderUpdateDTO orderUpdateDTO)
        {
            // 1. Buscar la orden existente en la base de datos
            var existingOrder = await _orderRepository.GetEntityById(id);

            // 2. Validar que exista y que su estado permita edición
            if (existingOrder == null) return null;

            if (existingOrder.OrderState == OrderState.Cerrada || existingOrder.OrderState == OrderState.Cancelada)
            {
                // Lanza una excepción o devuelve nulo (dependiendo de cómo manejes errores en tu controlador)
                throw new InvalidOperationException("No se puede editar esta orden porque está cerrada o cancelada.");
            }

            // (Opcional, agregar la regla de si está "Calculada" y evitar cambiar el SupplierId, etc.)

            // 3. Sobreescribir los datos viejos con los datos nuevos del DTO
            // Actualizamos solo lo que tiene sentido cambiar en una orden.
            existingOrder.ImporterId = orderUpdateDTO.ImporterId;
            existingOrder.SupplierId = orderUpdateDTO.SupplierId;
            existingOrder.OriginCountryId = orderUpdateDTO.OriginCountryId;
            existingOrder.CurrencyId = orderUpdateDTO.CurrencyId;
            existingOrder.TransportMode = orderUpdateDTO.TransportMode;

            // Opcional: Dependiendo de tu lógica, podrías o no permitir cambiar el estado desde este método
            // existingOrder.OrderState = orderUpdateDTO.OrderState; 

            // 4. Guardar los cambios en la base de datos
            await _orderRepository.EditAsync(existingOrder);

            // 5. Preparar la respuesta mapeando la entidad ya actualizada
            var response = new ImportationOrderResponseDTO
            {
                OrderId = existingOrder.OrderId,
                ImporterId = existingOrder.ImporterId,
                SupplierId = existingOrder.SupplierId,
                OriginCountryId = existingOrder.OriginCountryId,
                CurrencyId = existingOrder.CurrencyId,
                OrderDate = existingOrder.OrderDate, // Mantenemos la fecha original
                TransportMode = existingOrder.TransportMode,
                OrderState = existingOrder.OrderState,

                // Y por supuesto, mantenemos nuestros cálculos matemáticos intactos
                TotalFOB = existingOrder.ImportationOrderDetails != null
                    ? existingOrder.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice)
                    : 0,

                TotalImportationExpected =
                    (existingOrder.ImportationOrderDetails != null ? existingOrder.ImportationOrderDetails.Sum(d => d.Quantity * d.FOBUnitPrice) : 0) +
                    (existingOrder.ImportationExpenses != null ? existingOrder.ImportationExpenses.Sum(e => e.ExpenseAmount) : 0)
            };

            return response;
        }
    }
}