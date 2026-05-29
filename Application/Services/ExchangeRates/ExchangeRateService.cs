using Application.DTOs.ExchangeRates;
using Application.Interfaces.ExchangeRates;
using Application.Services.Result;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.ExchangeRates
{
    public class ExchangeRateService : IExchangeRateService
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

        public Task<ExchangeRateDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CreateAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}