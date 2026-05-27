using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Entities.FinancialCore;

namespace Application.Services.FinancialCore
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _repository;

        public CountryService(CountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<CountryDto>> GetAllAsync()
        {

            var countriesEntities = await _repository.GetAllAsync();
            var dtosList = countriesEntities.Select(entity => new CountryDto
            {
                Key = entity.Key,
                Name = entity.Name,
                IsoCode = entity.IsoCode,
                State = entity.State
            }).ToList(); 
            return dtosList;
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {
            var resultEntity = await _repository.GetEntityById(id);
            if (resultEntity == null)
            {
                return null;
                
            }

            return new CountryDto { Key = resultEntity.Key,
                Name = resultEntity.Name,
                IsoCode = resultEntity.IsoCode,
                State = resultEntity.State
            };
            
        }
        //--------------------------------------------
        public async Task<bool> CreateAsync(CountryDto dto)
        {
             dto.IsoCode = dto.IsoCode.Trim().ToUpper();
            var existingCountry = await _repository.GetByIsoCodeAsync(dto.IsoCode);
            if (existingCountry != null)
            {
                return false;
            }

            var entity = new Country
            {
                Key = 0,
                Name = dto.Name,
                IsoCode = dto.IsoCode,
                State = dto.State
            };

           return await _repository.CreateAsync(entity);
            

            
        }
        //--------------------------------------------



        //--------------------------------------------
        public async Task<bool> UpdateAsync(CountryDto dto)
        {
            dto.IsoCode = dto.IsoCode.Trim().ToUpper();
            var existing =  await _repository.GetEntityById(dto.Key);

            if (existing == null)
            {
                return false;  
            }

            var CountryWithThatIso = await _repository.GetByIsoCodeAsync(dto.IsoCode);

            if (CountryWithThatIso != null)
            {
                if (CountryWithThatIso .Key != dto.Key)
                {
                    return false;
                }

            }

            existing.Name = dto.Name;
            existing.IsoCode = dto.IsoCode;
            existing.State = dto.State;



            return await _repository.EditAsync(existing);



        }
        //--------------------------------------------

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}