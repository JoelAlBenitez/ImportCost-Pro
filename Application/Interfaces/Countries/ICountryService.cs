using Application.DTOs.Countries;
using Application.Services.Result;

namespace Application.Interfaces.Countries
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