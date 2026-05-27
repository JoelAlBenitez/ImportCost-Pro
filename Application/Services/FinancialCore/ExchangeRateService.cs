using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.FinancialCore
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

        public Task<bool> CreateAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ExchangeRateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}