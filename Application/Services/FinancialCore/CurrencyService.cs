using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.FinancialCore
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

        public Task<bool> CreateAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}