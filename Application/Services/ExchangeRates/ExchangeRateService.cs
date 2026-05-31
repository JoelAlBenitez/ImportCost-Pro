using Application.DTOs.ExchangeRates;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.ImportationOrderAndLandCost;

namespace Application.Services.ExchangeRates
{
    public class ExchangeRateService : IServicesBase<ExchangeRateDto, int>
    {
        private readonly ExchangeRateRepository _repository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly ImportationOrderRepository _ordersRepository;

        public ExchangeRateService(
            ExchangeRateRepository repository,
            CurrencyRepository currencyRepository,
            ImportationOrderRepository ordersRepository)
        {
            _repository = repository;
            _currencyRepository = currencyRepository;
            _ordersRepository = ordersRepository;
        }

        public async Task<IReadOnlyCollection<ExchangeRateDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new ExchangeRateDto
            {
                Key = e.Key,
                SourceCurrencyId = e.SourceCurrencyId,
                SourceCurrencyName = e.SourceCurrency?.Name,
                DestinationCurrencyId = e.DestinationCurrencyId,
                DestinationCurrencyName = e.DestinationCurrency?.Name,
                RateValue = e.RateValue,
                EffectiveDate = e.EffectiveDate,
                State = e.State
            }).ToList();
        }

        public async Task<ExchangeRateDto> GetKeyAsync(int id)
        {
            var e = await _repository.GetEntityById(id);
            if (e == null) return null!;
            return new ExchangeRateDto
            {
                Key = e.Key,
                SourceCurrencyId = e.SourceCurrencyId,
                SourceCurrencyName = e.SourceCurrency?.Name,
                DestinationCurrencyId = e.DestinationCurrencyId,
                DestinationCurrencyName = e.DestinationCurrency?.Name,
                RateValue = e.RateValue,
                EffectiveDate = e.EffectiveDate,
                State = e.State
            };
        }

        public async Task<ServiceResult> CreateAsync(ExchangeRateDto dto)
        {
            try
            {
                if (dto.SourceCurrencyId == dto.DestinationCurrencyId)
                {
                    return new ServiceResult { Success = false, Message = "La moneda origen no puede ser igual a la moneda destino.", TypeAlert = "danger" };
                }

                if (dto.RateValue <= 0)
                {
                    return new ServiceResult { Success = false, Message = "El valor de la tasa debe ser mayor que 0.", TypeAlert = "danger" };
                }

                var source = await _currencyRepository.GetEntityById(dto.SourceCurrencyId);
                var dest = await _currencyRepository.GetEntityById(dto.DestinationCurrencyId);

                if (source == null || !source.State || dest == null || !dest.State)
                {
                    return new ServiceResult { Success = false, Message = "Una de las monedas seleccionadas no es válida o está inactiva.", TypeAlert = "danger" };
                }

                var existing = await _repository.GetLatestRateAsync(dto.SourceCurrencyId, dto.DestinationCurrencyId, dto.EffectiveDate);
                if (existing != null && existing.EffectiveDate.Date == dto.EffectiveDate.Date && existing.State)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe una tasa de cambio activa para esta moneda origen, moneda destino y fecha de vigencia.", TypeAlert = "danger" };
                }

                var entity = new ExchangeRate
                {
                    Key = 0,
                    Name = $"{source.IsoCode} -> {dest.IsoCode}",
                    SourceCurrencyId = dto.SourceCurrencyId,
                    DestinationCurrencyId = dto.DestinationCurrencyId,
                    RateValue = dto.RateValue,
                    EffectiveDate = dto.EffectiveDate,
                    State = dto.State,
                    SourceCurrency = null!,
                    DestinationCurrency = null!
                };

                var result = await _repository.CreateAsync(entity);
                if (result) return new ServiceResult { Success = true, Message = "Tasa de cambio registrada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al registrar la tasa de cambio.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error técnico inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> EditAsync(ExchangeRateDto dto)
        {
            try
            {
                var existing = await _repository.GetEntityById(dto.Key);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "La tasa de cambio que intenta actualizar no existe.", TypeAlert = "danger" };
                }

                // Paso 1: Identificar cambios críticos (Pág. 66)
                bool isCriticalChanged = existing.SourceCurrencyId != dto.SourceCurrencyId ||
                                         existing.DestinationCurrencyId != dto.DestinationCurrencyId ||
                                         existing.RateValue != dto.RateValue ||
                                         existing.EffectiveDate.Date != dto.EffectiveDate.Date;

                // Paso 2: Validación Preventiva si hubo cambios críticos
                if (isCriticalChanged)
                {
                    bool inUse = await _ordersRepository.IsRateInUseAsync(existing);
                    if (inUse)
                    {
                        return new ServiceResult { Success = false, Message = "No se puede modificar esta tasa de cambio porque ya fue utilizada en un cálculo oficial de landed cost.", TypeAlert = "danger" };
                    }
                }

                // Paso 3: Validaciones básicas
                if (dto.SourceCurrencyId == dto.DestinationCurrencyId)
                {
                    return new ServiceResult { Success = false, Message = "La moneda origen no puede ser igual a la moneda destino.", TypeAlert = "danger" };
                }

                var source = await _currencyRepository.GetEntityById(dto.SourceCurrencyId);
                var dest = await _currencyRepository.GetEntityById(dto.DestinationCurrencyId);
                
                existing.SourceCurrencyId = dto.SourceCurrencyId;
                existing.DestinationCurrencyId = dto.DestinationCurrencyId;
                existing.RateValue = dto.RateValue;
                existing.EffectiveDate = dto.EffectiveDate;
                existing.State = dto.State;
                existing.Name = $"{source?.IsoCode} -> {dest?.IsoCode}";

                var result = await _repository.EditAsync(existing);
                if (result) return new ServiceResult { Success = true, Message = "Tasa de cambio actualizada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al actualizar la tasa de cambio.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error técnico inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetEntityById(id);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "La tasa de cambio que intenta eliminar no existe.", TypeAlert = "danger" };
                }

                // Validación Preventiva (Pág. 68)
                bool inUse = await _ordersRepository.IsRateInUseAsync(existing);
                if (inUse)
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar esta tasa de cambio porque ya fue utilizada en un cálculo oficial de landed cost.", TypeAlert = "danger" };
                }

                var result = await _repository.DeleteAsync(id);
                if (result) return new ServiceResult { Success = true, Message = "Tasa de cambio eliminada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al eliminar la tasa de cambio.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error técnico inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }
    }
}
