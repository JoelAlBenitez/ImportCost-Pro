using Application.DTOs.TaxConfigurations;
using Application.Services.Result;

namespace Application.Interfaces.TaxConfigurations
{
    public interface ITaxConfigurationService
    {
        Task<IReadOnlyCollection<TaxConfigurationDto>> GetAllAsync();
        Task<TaxConfigurationDto?> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(TaxConfigurationDto dto);
        Task<ServiceResult> UpdateAsync(TaxConfigurationDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}