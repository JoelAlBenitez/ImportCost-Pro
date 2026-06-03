using Application.DTOs.Expenses;
using Application.Services.Result;
using Persistence.Entities.Enums;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Repositories.ImportationOrderAndLandCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.ImportationExpenseServices
{
    public class ImportationExpenseService
    {
        private readonly ImportationOrderRepository _orderRepository;
        private readonly ImportationExpenseRepository _expenseRepository;

        public ImportationExpenseService(
            ImportationOrderRepository orderRepository,
            ImportationExpenseRepository expenseRepository)
        {
            _orderRepository = orderRepository;
            _expenseRepository = expenseRepository;
        }
        public async Task<List<ImportationExpenseResponseDTO>> GetExpensesByOrderIdAsync(string orderId)
        {
            var order = await _orderRepository.GetEntityById(orderId);

            if (order == null || order.ImportationExpenses == null)
                return new List<ImportationExpenseResponseDTO>();

            return order.ImportationExpenses.Select(e => new ImportationExpenseResponseDTO
            {
                ImportationExpenseId = e.ImportationExpenseId,
                OrderId = e.ImportationOrderId,
                ExpenseType = e.ExpenseType,
                ExpenseAmount = e.ExpenseAmount,
                CurrencyId = e.CurrencyId,
                DistributionMethod = e.DistributionMethod,
                ExpenseDate = e.ExpenseDate
            }).ToList();
        }

        public async Task<ImportationExpenseResponseDTO?> GetExpenseByIdAsync(string expenseId)
        {
            var expense = await _expenseRepository.GetEntityById(expenseId);
            if (expense == null)
            {
                return null;
            }

            return new ImportationExpenseResponseDTO
            {
                ImportationExpenseId = expense.ImportationExpenseId,
                OrderId = expense.ImportationOrderId,
                ExpenseType = expense.ExpenseType,
                ExpenseAmount = expense.ExpenseAmount,
                CurrencyId = expense.CurrencyId,
                DistributionMethod = expense.DistributionMethod,
                ExpenseDate = expense.ExpenseDate
            };
        }


        public async Task<ServiceResult> AddExpenseToOrderAsync(ImportationExpenseCreateDTO dto)
        {
            var order = await _orderRepository.GetEntityById(dto.OrderId);

            if (order == null)
                return new ServiceResult { Success = false, Message = "La orden no existe.", TypeAlert = "error" };

            if (order.OrderState != OrderState.Abierta)
                return new ServiceResult { Success = false, Message = "Solo se pueden agregar gastos a órdenes en estado Abierta.", TypeAlert = "warning" };

            if (dto.ExpenseAmount <= 0)
                return new ServiceResult { Success = false, Message = "El monto del gasto debe ser mayor a cero.", TypeAlert = "warning" };

            var newExpense = new ImportationExpense
            {
                ImportationExpenseId = Guid.NewGuid().ToString(),
                ImportationOrderId = dto.OrderId,
                ExpenseType = dto.ExpenseType,
                ExpenseAmount = dto.ExpenseAmount,
                CurrencyId = dto.CurrencyId,
                DistributionMethod = dto.DistributionMethod,

                ImportationExpenseDate = dto.ExpenseDate,
                ImportationOrder = null!,
                Currency = null!
            };

            await _expenseRepository.CreateAsync(newExpense);

            return new ServiceResult { Success = true, Message = "Gasto registrado correctamente.", TypeAlert = "success" };
        }

        public async Task<ServiceResult> EditExpenseAsync(ImportationExpenseUpdateDTO dto)
        {
            var expense = await _expenseRepository.GetEntityById(dto.ImportationExpenseId);

            if (expense == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "El gasto no fue encontrado.",
                    TypeAlert = "error"
                };
            }

            var order = await _orderRepository.GetEntityById(expense.ImportationOrderId);
            if (order?.OrderState != OrderState.Abierta)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "No se puede editar este gasto porque la orden ya no está Abierta.",
                    TypeAlert = "warning"
                };
            }
            expense.ExpenseType = dto.ExpenseType;
            expense.ExpenseAmount = dto.ExpenseAmount;
            expense.CurrencyId = dto.CurrencyId;
            expense.DistributionMethod = dto.DistributionMethod;
            expense.ExpenseDate = dto.ExpenseDate;

            await _expenseRepository.EditAsync(expense);
            return new ServiceResult
            {
                Success = true,
                Message = "Gasto actualizado correctamente.",
                TypeAlert = "success"
            };
        }
        // ELIMINAR UN GASTO DELETE

        public async Task<ServiceResult> RemoveExpenseAsync(string expenseId)
        {
            var expense = await _expenseRepository.GetEntityById(expenseId);
            if (expense == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "El gasto de importación no fue encontrado.",
                    TypeAlert = "error"
                };
            }
            var order = await _orderRepository.GetEntityById(expense.ImportationOrderId);

            if (order?.OrderState != OrderState.Abierta)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "No se puede eliminar este gasto porque la orden ya no está Abierta.",
                    TypeAlert = "warning"
                };
            }

            await _expenseRepository.DeleteAsync(expenseId);

            return new ServiceResult
            {
                Success = true,
                Message = "El gasto ha sido eliminado correctamente de la orden.",
                TypeAlert = "success"
            };
        }
    }
}