using Application.DTOs.FinancialCore;
using Application.Services.Result;

namespace Application.Interfaces.FinancialCore
{
    public interface ICountryService
    {
        Task<IReadOnlyCollection<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CountryDto dto);
        Task<ServiceResult> UpdateAsync(CountryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}