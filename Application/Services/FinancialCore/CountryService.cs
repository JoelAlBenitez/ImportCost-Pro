using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.FinancialCore
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _repository;

        public CountryService(CountryRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<CountryDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CountryDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(CountryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CountryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}