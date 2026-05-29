using Application.DTOs.Currencies;
using Application.Interfaces.Currencies;
using Application.Services.Result;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.Currencies
{
    public class CurrencyService : ICurrencyService
    {
        private readonly CurrencyRepository _repository;

        public CurrencyService(CurrencyRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<CurrencyDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CreateAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}