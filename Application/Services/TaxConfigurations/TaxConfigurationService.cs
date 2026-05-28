using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.TaxConfigurations
{
    public class TaxConfigurationService : ITaxConfigurationService
    {
        private readonly TaxConfigurationRepository _repository;

        public TaxConfigurationService(TaxConfigurationRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<TaxConfigurationDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TaxConfigurationDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}