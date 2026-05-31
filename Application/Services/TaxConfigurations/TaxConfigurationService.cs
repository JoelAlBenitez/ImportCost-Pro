using Application.DTOs.TaxConfigurations;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.FinancialCore;
using Persistence.Repositories.FinancialCore;

namespace Application.Services.TaxConfigurations
{
    public class TaxConfigurationService : IServicesBase<TaxConfigurationDto, int>
    {
        private readonly TaxConfigurationRepository _repository;

        public TaxConfigurationService(TaxConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<TaxConfigurationDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new TaxConfigurationDto
            {
                Key = e.Key,
                Name = e.Name,
                GeneralItbisPercentage = e.GeneralItbisPercentage,
                CustomsServiceRatePercentage = e.CustomsServiceRatePercentage,
                State = e.State
            }).ToList();
        }

        public async Task<TaxConfigurationDto> GetKeyAsync(int id)
        {
            var e = await _repository.GetEntityById(id);
            if (e == null) return null!;
            return new TaxConfigurationDto
            {
                Key = e.Key,
                Name = e.Name,
                GeneralItbisPercentage = e.GeneralItbisPercentage,
                CustomsServiceRatePercentage = e.CustomsServiceRatePercentage,
                State = e.State
            };
        }

        public async Task<ServiceResult> CreateAsync(TaxConfigurationDto dto)
        {
            try
            {
               
                if (dto.GeneralItbisPercentage < 0 || dto.GeneralItbisPercentage > 100 ||
                    dto.CustomsServiceRatePercentage < 0 || dto.CustomsServiceRatePercentage > 100)
                {
                    return new ServiceResult { Success = false, Message = "Los porcentajes de impuestos deben estar entre 0 y 100.", TypeAlert = "danger" };
                }

                
                if (dto.State)
                {
                    var existingActive = await _repository.GetCurrentConfigAsync();
                    if (existingActive != null)
                    {
                        return new ServiceResult 
                        { 
                            Success = false, 
                            Message = "Ya existe una configuración de impuestos activa en el sistema. Debe inactivar la actual antes de activar una nueva.", 
                            TypeAlert = "danger" 
                        };
                    }
                }

                var entity = new TaxConfiguration
                {
                    Key = 0,
                    Name = dto.Name,
                    GeneralItbisPercentage = dto.GeneralItbisPercentage,
                    CustomsServiceRatePercentage = dto.CustomsServiceRatePercentage,
                    State = dto.State
                };

                var result = await _repository.CreateAsync(entity);
                if (result)
                    return new ServiceResult { Success = true, Message = "Configuración de impuestos registrada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al registrar la configuración.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> EditAsync(TaxConfigurationDto dto)
        {
            try
            {
                
                if (dto.GeneralItbisPercentage < 0 || dto.GeneralItbisPercentage > 100 ||
                    dto.CustomsServiceRatePercentage < 0 || dto.CustomsServiceRatePercentage > 100)
                {
                    return new ServiceResult { Success = false, Message = "Los porcentajes de impuestos deben estar entre 0 y 100.", TypeAlert = "danger" };
                }

                var existing = await _repository.GetEntityById(dto.Key);
                if (existing == null)
                {
                    return new ServiceResult { Success = false, Message = "La configuración que intenta actualizar no existe.", TypeAlert = "danger" };
                }

             
                if (!existing.State && dto.State)
                {
                    var existingActive = await _repository.GetCurrentConfigAsync();
                    if (existingActive != null && existingActive.Key != dto.Key)
                    {
                        return new ServiceResult 
                        { 
                            Success = false, 
                            Message = "Ya existe otra configuración de impuestos activa. Inactiva la actual antes de activar esta.", 
                            TypeAlert = "danger" 
                        };
                    }
                }

                existing.Name = dto.Name;
                existing.GeneralItbisPercentage = dto.GeneralItbisPercentage;
                existing.CustomsServiceRatePercentage = dto.CustomsServiceRatePercentage;
                existing.State = dto.State;

                var result = await _repository.EditAsync(existing);
                if (result)
                    return new ServiceResult { Success = true, Message = "Configuración de impuestos actualizada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al actualizar la configuración.", TypeAlert = "danger" };
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
                    return new ServiceResult { Success = false, Message = "La configuración que intenta eliminar no existe.", TypeAlert = "danger" };
                }

               
                if (existing.State)
                {
                    return new ServiceResult { Success = false, Message = "No se puede eliminar una configuración de impuestos que esté activa.", TypeAlert = "danger" };
                }

               

                var result = await _repository.DeleteAsync(id);
                if (result)
                    return new ServiceResult { Success = true, Message = "Configuración eliminada con éxito.", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al eliminar la configuración.", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error inesperado: {ex.Message}", TypeAlert = "danger" };
            }
        }
    }
}
