using Application.DTOs.Countries;
using Application.Interfaces.Countries;
using Application.Services.Result;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.Countries
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _repository;
        private readonly ImportersRepository _importersRepository;
        private readonly SuppliersRepository _suppliersRepository;
        private readonly ProductsRepository _productsRepository;

        public CountryService(CountryRepository repository, 
                              ImportersRepository importersRepository, 
                              SuppliersRepository suppliersRepository, 
                              ProductsRepository productsRepository)
        {
            _repository = repository;
            _importersRepository = importersRepository;
            _suppliersRepository = suppliersRepository;
            _productsRepository = productsRepository;
        }

        public async Task<IReadOnlyCollection<CountryDto>> GetAllAsync()
        {
            var countriesEntities = await _repository.GetAllAsync();
            return countriesEntities.Select(entity => new CountryDto
            {
                Key = entity.Key,
                Name = entity.Name,
                IsoCode = entity.IsoCode,
                State = entity.State
            }).ToList();
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {
            var resultEntity = await _repository.GetEntityById(id);
            if (resultEntity == null) return null;

            return new CountryDto
            {
                Key = resultEntity.Key,
                Name = resultEntity.Name,
                IsoCode = resultEntity.IsoCode,
                State = resultEntity.State
            };
        }

        public async Task<ServiceResult> CreateAsync(CountryDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.IsoCode))
                {
                    return new ServiceResult { Success = false, Message = "El nombre y el código ISO son obligatorios.", TypeAlert = "danger" };
                }

                if (dto.IsoCode.Trim().Length < 2 || dto.IsoCode.Trim().Length > 3)
                {
                    return new ServiceResult { Success = false, Message = "El código ISO debe tener entre 2 y 3 caracteres.", TypeAlert = "danger" };
                }

                dto.IsoCode = dto.IsoCode.Trim().ToUpper();
                var existingCountry = await _repository.GetByIsoCodeAsync(dto.IsoCode);
                if (existingCountry != null)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe un país registrado con este código ISO.", TypeAlert = "danger" };
                }

                var entity = new Country
                {
                    Key = 0,
                    Name = dto.Name,
                    IsoCode = dto.IsoCode,
                    State = dto.State
                };

                var result = await _repository.CreateAsync(entity);
                if (result)
                    return new ServiceResult { Success = true, Message = "País registrado con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al registrar el país.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> UpdateAsync(CountryDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.IsoCode))
                {
                    return new ServiceResult { Success = false, Message = "El nombre y el código ISO son obligatorios.", TypeAlert = "danger" };
                }

                if (dto.IsoCode.Trim().Length < 2 || dto.IsoCode.Trim().Length > 3)
                {
                    return new ServiceResult { Success = false, Message = "El código ISO debe tener entre 2 y 3 caracteres.", TypeAlert = "danger" };
                }

                dto.IsoCode = dto.IsoCode.Trim().ToUpper();
                var existing = await _repository.GetEntityById(dto.Key);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "El país que intenta actualizar no existe.", TypeAlert = "danger" };
                }

                var countryWithThatIso = await _repository.GetByIsoCodeAsync(dto.IsoCode);
                if (countryWithThatIso != null && countryWithThatIso.Key != dto.Key)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe un país registrado con este código ISO.", TypeAlert = "danger" };
                }

                existing.Name = dto.Name;
                existing.IsoCode = dto.IsoCode;
                existing.State = dto.State;

                var result = await _repository.EditAsync(existing);
                if (result)
                    return new ServiceResult { Success = true, Message = "País actualizado con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al actualizar el país.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetEntityById(id);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "El país que intenta eliminar no existe.", TypeAlert = "danger" };
                }

                var hasImporters = await _importersRepository.HasImportersByCountryId(id);
                var hasSuppliers = await _suppliersRepository.HasSuppliersByCountryId(id);
                var hasProducts = await _productsRepository.HasProductsByCountryId(id);

                if (hasImporters || hasSuppliers || hasProducts)
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar este país porque está asociado a otros registros del sistema.", TypeAlert = "danger" };
                }

                var result = await _repository.DeleteAsync(existing.Key);
                if (result)
                    return new ServiceResult { Success = true, Message = "País eliminado con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al eliminar el país.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }
    }
}