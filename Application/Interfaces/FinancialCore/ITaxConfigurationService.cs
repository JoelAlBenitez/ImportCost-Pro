using Application.DTOs.FinancialCore;

namespace Application.Interfaces.FinancialCore
{
    public interface ITaxConfigurationService
    {
        Task<IReadOnlyCollection<TaxConfigurationDto>> GetAllAsync();
        Task<TaxConfigurationDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(TaxConfigurationDto dto);
        Task<bool> UpdateAsync(TaxConfigurationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}