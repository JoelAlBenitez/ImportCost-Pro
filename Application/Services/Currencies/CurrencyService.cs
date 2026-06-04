using Application.DTOs.Currencies;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;
using Persistence.Repositories.ImportationOrderAndLandCost;

namespace Application.Services.Currencies
{
    public class CurrencyService : IServicesBase<CurrencyDto, int>
    {
        private readonly CurrencyRepository _repository;
        private readonly SuppliersRepository _suppliersRepository;
        private readonly ExchangeRateRepository _exchangeRateRepository;
        private readonly ImportationOrderRepository _ordersRepository;
        private readonly ImportationExpenseRepository _expensesRepository;

        public CurrencyService(
            CurrencyRepository repository,
            SuppliersRepository suppliersRepository,
            ExchangeRateRepository exchangeRateRepository,
            ImportationOrderRepository ordersRepository,
            ImportationExpenseRepository expensesRepository)
        {
            _repository = repository;
            _suppliersRepository = suppliersRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _ordersRepository = ordersRepository;
            _expensesRepository = expensesRepository;
        }

        public async Task<IReadOnlyCollection<CurrencyDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new CurrencyDto
            {
                Key = e.Key,
                Name = e.Name,
                IsoCode = e.IsoCode,
                Symbol = e.Symbol,
                IsLocalCurrency = e.IsLocalCurrency,
                State = e.State
            }).ToList();
        }

        public async Task<CurrencyDto> GetKeyAsync(int id)
        {
            var e = await _repository.GetEntityById(id);
            if (e == null) return null!;
            return new CurrencyDto
            {
                Key = e.Key,
                Name = e.Name,
                IsoCode = e.IsoCode,
                Symbol = e.Symbol,
                IsLocalCurrency = e.IsLocalCurrency,
                State = e.State
            };
        }

        public async Task<ServiceResult> CreateAsync(CurrencyDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.IsoCode) || dto.IsoCode.Trim().Length != 3)
                {
                    return new ServiceResult { Success = false, Message = "El código ISO debe tener exactamente 3 caracteres.", TypeAlert = "danger" };
                }

                dto.IsoCode = dto.IsoCode.Trim().ToUpper();

                var existingWithIso = await _repository.GetByIsoCodeAsync(dto.IsoCode);
                if (existingWithIso != null)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe una moneda registrada con este código ISO.", TypeAlert = "danger" };
                }

                if (dto.IsLocalCurrency)
                {
                    var existingLocal = await _repository.GetLocalCurrencyAsync();
                    if (existingLocal != null)
                    {
                        return new ServiceResult { Success = false, Message = "Ya existe una moneda configurada como moneda local. Solo puede existir una moneda local en el sistema.", TypeAlert = "danger" };
                    }
                }

                var entity = new Currency
                {
                    Key = 0,
                    Name = dto.Name,
                    IsoCode = dto.IsoCode,
                    Symbol = dto.Symbol,
                    IsLocalCurrency = dto.IsLocalCurrency,
                    State = dto.State
                };

                var result = await _repository.CreateAsync(entity);
                if (result) return new ServiceResult { Success = true, Message = "Moneda registrada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al registrar la moneda.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> EditAsync(CurrencyDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.IsoCode) || dto.IsoCode.Trim().Length != 3)
                {
                    return new ServiceResult { Success = false, Message = "El código ISO debe tener exactamente 3 caracteres.", TypeAlert = "danger" };
                }

                dto.IsoCode = dto.IsoCode.Trim().ToUpper();

                var existing = await _repository.GetEntityById(dto.Key);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "La moneda que intenta actualizar no existe.", TypeAlert = "danger" };
                }

                var existingWithIso = await _repository.GetByIsoCodeAsync(dto.IsoCode);
                if (existingWithIso != null && existingWithIso.Key != dto.Key)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe una moneda registrada con este código ISO.", TypeAlert = "danger" };
                }

                if (existing.IsoCode != dto.IsoCode)
                {
                    bool inUse = await _suppliersRepository.HasSuppliersByCurrencyId(dto.Key) ||
                                 await _exchangeRateRepository.HasExchangeRatesByCurrencyId(dto.Key) ||
                                 await _ordersRepository.HasOrdersByCurrencyId(dto.Key) ||
                                 await _expensesRepository.HasExpensesByCurrencyId(dto.Key);
                    
                    if (inUse)
                    {
                        return new ServiceResult { Success = false, Message = "No se puede modificar el código ISO porque esta moneda ya está asociada a proveedores, tasas, órdenes o gastos.", TypeAlert = "danger" };
                    }
                }

                if (!existing.IsLocalCurrency && dto.IsLocalCurrency)
                {
                    var existingLocal = await _repository.GetLocalCurrencyAsync();
                    if (existingLocal != null && existingLocal.Key != dto.Key)
                    {
                        return new ServiceResult { Success = false, Message = "Ya existe una moneda configurada como moneda local. Solo puede existir una moneda local en el sistema.", TypeAlert = "danger" };
                    }
                }

                if (existing.IsLocalCurrency && !dto.State)
                {
                    bool inUse = await _suppliersRepository.HasSuppliersByCurrencyId(dto.Key) ||
                                 await _exchangeRateRepository.HasExchangeRatesByCurrencyId(dto.Key) ||
                                 await _ordersRepository.HasOrdersByCurrencyId(dto.Key) ||
                                 await _expensesRepository.HasExpensesByCurrencyId(dto.Key);

                    if (inUse)
                    {
                        return new ServiceResult { Success = false, Message = "No se puede inactivar la moneda local mientras existan registros que dependan de ella.", TypeAlert = "danger" };
                    }
                }

                existing.Name = dto.Name;
                existing.IsoCode = dto.IsoCode;
                existing.Symbol = dto.Symbol;
                existing.IsLocalCurrency = dto.IsLocalCurrency;
                existing.State = dto.State;

                var result = await _repository.EditAsync(existing);
                if (result) return new ServiceResult { Success = true, Message = "Moneda actualizada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al actualizar la moneda.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetEntityById(id);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "La moneda que intenta eliminar no existe.", TypeAlert = "danger" };
                }

                if (existing.IsLocalCurrency)
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar la moneda local del sistema.", TypeAlert = "danger" };
                }

                bool inUse = await _suppliersRepository.HasSuppliersByCurrencyId(id) ||
                             await _exchangeRateRepository.HasExchangeRatesByCurrencyId(id) ||
                             await _ordersRepository.HasOrdersByCurrencyId(id) ||
                             await _expensesRepository.HasExpensesByCurrencyId(id);

                if (inUse)
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar esta moneda porque está en uso por proveedores, tasas, órdenes o gastos.", TypeAlert = "danger" };
                }

                var result = await _repository.DeleteAsync(id);
                if (result) return new ServiceResult { Success = true, Message = "Moneda eliminada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al eliminar la moneda.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }
    }
}
