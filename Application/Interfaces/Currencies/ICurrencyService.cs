using Application.DTOs.Currencies;
using Application.Services.Result;

namespace Application.Interfaces.Currencies
{
    public interface ICurrencyService
    {
        Task<IReadOnlyCollection<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto?> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CurrencyDto dto);
        Task<ServiceResult> UpdateAsync(CurrencyDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}