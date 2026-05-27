using Application.DTOs.FinancialCore;

namespace Application.Interfaces.FinancialCore
{
    public interface ICountryService
    {
        Task<IReadOnlyCollection<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CountryDto dto);
        Task<bool> UpdateAsync(CountryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}