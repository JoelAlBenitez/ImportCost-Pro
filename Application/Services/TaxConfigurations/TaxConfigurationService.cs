using Application.DTOs.TaxConfigurations;
using Application.Interfaces.TaxConfigurations;
using Application.Services.Result;
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

        public Task<ServiceResult> CreateAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}