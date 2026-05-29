using Application.DTOs.FinancialCore;

namespace Application.Interfaces.FinancialCore
{
    public interface IExchangeRateService
    {
        Task<IReadOnlyCollection<ExchangeRateDto>> GetAllAsync();
        Task<ExchangeRateDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ExchangeRateDto dto);
        Task<bool> UpdateAsync(ExchangeRateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}