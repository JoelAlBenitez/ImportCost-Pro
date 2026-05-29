using Application.DTOs.ExchangeRates;
using Application.Services.Result;

namespace Application.Interfaces.ExchangeRates
{
    public interface IExchangeRateService
    {
        Task<IReadOnlyCollection<ExchangeRateDto>> GetAllAsync();
        Task<ExchangeRateDto?> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(ExchangeRateDto dto);
        Task<ServiceResult> UpdateAsync(ExchangeRateDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}