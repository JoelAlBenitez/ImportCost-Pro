using Application.DTOs.Countries;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.Countries
{
    public class CountryService : IServicesBase<CountryDto, int>
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

        public async Task<CountryDto> GetKeyAsync(int id)
        {
            var resultEntity = await _repository.GetEntityById(id);
            if (resultEntity == null) return null!;

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
                dto.IsoCode = dto.IsoCode.Trim().ToUpper();
                dto.Name = dto.Name.Trim();

                var existingCountry = await _repository.GetByIsoCodeAsync(dto.IsoCode);
                if (existingCountry != null)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe un país registrado con este código ISO.", TypeAlert = "danger" };
                }

                var existingWithName = await _repository.GetByNameAsync(dto.Name);
                if (existingWithName != null)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe un país registrado con este nombre.", TypeAlert = "danger" };
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

        public async Task<ServiceResult> EditAsync(CountryDto dto)
        {
            try
            {
                dto.IsoCode = dto.IsoCode.Trim().ToUpper();
                dto.Name = dto.Name.Trim();

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

                var existingWithName = await _repository.GetByNameAsync(dto.Name);
                if (existingWithName != null && existingWithName.Key != dto.Key)
                {
                    return new ServiceResult { Success = false, Message = "Ya existe un país registrado con este nombre.", TypeAlert = "danger" };
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

                // Desglose de validaciones para ser honestos con el usuario (Calidad Joel)
                if (await _importersRepository.HasImportersByCountryId(id))
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar este país porque tiene Importadores asociados.", TypeAlert = "danger" };
                }

                if (await _suppliersRepository.HasSuppliersByCountryId(id))
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar este país porque tiene Proveedores asociados.", TypeAlert = "danger" };
                }

                if (await _productsRepository.HasProductsByCountryId(id))
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar este país porque tiene Productos asociados.", TypeAlert = "danger" };
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