using Application.DTOs.FinancialCore;

namespace Application.Interfaces.FinancialCore
{
    public interface ICurrencyService
    {
        Task<IReadOnlyCollection<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CurrencyDto dto);
        Task<bool> UpdateAsync(CurrencyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}