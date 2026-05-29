using Application.DTOs.TaxConfigurations;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.TaxConfigurations
{
    public class TaxConfigurationService : IServicesBase<TaxConfigurationDto, int>
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

        public Task<TaxConfigurationDto> GetKeyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CreateAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> EditAsync(TaxConfigurationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}