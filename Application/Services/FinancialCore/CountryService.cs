using Application.DTOs.FinancialCore;
using Application.Interfaces.FinancialCore;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.FinancialCore
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _repository;
        private readonly ImportersRepository _importersRepository;
        private readonly SuppliersRepository _suppliersRepository;
        private readonly ProductsRepository _productsRepository;   

        public CountryService(CountryRepository repository, ImportersRepository importersRepository, SuppliersRepository suppliersRepository, ProductsRepository productsRepository)
        {
            _repository = repository;
            _importersRepository = importersRepository;
            _suppliersRepository = suppliersRepository;
            _productsRepository = productsRepository;
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

            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.IsoCode))
            {
                return false;
            }

            if (dto.IsoCode.Trim().Length < 2)
            {
                return false;
            } 




            dto.IsoCode = dto.IsoCode.Trim().ToUpper();
            var existingCountry = await _repository.GetByIsoCodeAsync(dto.IsoCode);
            if (existingCountry != null)
            {
                throw new Exception("Ya existe un país registrado con este código ISO.");
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

            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.IsoCode))
            {
                return false;
            }


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
                    throw new Exception("Ya existe un país registrado con este código ISO.");
                }

            }

            existing.Name = dto.Name;
            existing.IsoCode = dto.IsoCode;
            existing.State = dto.State;



            return await _repository.EditAsync(existing);



        }
        //--------------------------------------------

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetEntityById(id);
            if (existing == null) return false;

            var hasImporters = await _importersRepository.HasImportersByCountryId(id);
            var hasSuppliers = await _suppliersRepository.HasSuppliersByCountryId(id);
            var hasProducts = await _productsRepository.HasProductsByCountryId(id);
            // var hasOrders = await _importOrdersRepository.HasOrdersByCountryId(id);

            // Regla de negocio de la Pg 8: ignore estas excepciones, las hare con el patron result en ves de lanzarlas diretamente
            if (hasImporters || hasSuppliers || hasProducts /* || hasOrders */)
            {
                throw new Exception("No se puede eliminar este país porque está asociado a otros registros del sistema.");
            }

            return await _repository.DeleteAsync(existing.Key);
        }
    }
}