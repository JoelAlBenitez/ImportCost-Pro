using Application.DTOs.Currencies;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.Currencies
{
    public class CurrencyService : IServicesBase<CurrencyDto, int>
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

        public Task<CurrencyDto> GetKeyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> CreateAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> EditAsync(CurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}