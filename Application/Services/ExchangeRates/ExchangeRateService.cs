using Application.DTOs.ExchangeRates;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.ExchangeRates
{
    public class ExchangeRateService : IServicesBase<ExchangeRateDto, int>
    {
        private readonly ExchangeRateRepository _repository;

        public ExchangeRateService(ExchangeRateRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<ExchangeRateDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ExchangeRateDto> GetKeyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CreateAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> EditAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}