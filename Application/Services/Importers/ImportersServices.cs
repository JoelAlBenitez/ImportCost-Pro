using Application.DTOs.Importers;

using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.Importers
{
    public class ImportersServices : IServicesBase<ImporterDto, int>
    {
        private readonly ImportersRepository _importersRepository;
        public ImportersServices (ImportersRepository repository)
        {
            _importersRepository = repository;
        }

        public async Task<bool> ExistRnc(string Identification)
        {
            try
            {
                return await _importersRepository.ExistImportersByRnc(Identification);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ServiceResult> CreateAsync(ImporterDto dto)
        {
            try
            {
                Persistence.Entities.OperationalCommercial.Importers importers = new()
                {
                    Name = dto.Name,
                    State = dto.State,
                    Identification = dto.Identification,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address,
                    countryId = dto.CountryId
                };
                var exits = await ExistRnc(importers.Identification);
                if (exits) return new ServiceResult {Success = false, Message ="Ya existe un importador con esta identificacion", TypeAlert = "danger"};

                bool create = await _importersRepository.CreateAsync(importers);
                if (create) return new ServiceResult { Success = true, Message = "Importador registrado con exito", TypeAlert = "success" };

                return new ServiceResult { Success = false, Message = "Ha ocurrido un error en la creacion del importador", TypeAlert = "danger"};
                
            }catch(Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion con el servicio {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> DeleteAsync(int key)
        {
            try
            {
                bool exitsImp = await _importersRepository.AssociatedImportationOrderByImporters(key);
                if (exitsImp) return new ServiceResult { Success = false, Message = "Este importador tiene ordenes de importación asociadas por lo que no se puede eliminar", TypeAlert= "danger"};

                bool delete =  await _importersRepository.DeleteAsync(key);
                if (delete) return new ServiceResult { Success = true, Message = "Importador eliminado con extio", TypeAlert = "success" };
                return new ServiceResult {Success = false, Message = "Ha ocurrido un error al eliminar el importador", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion del servicio {ex.Message}", TypeAlert = "danger"};
            }
        }

        public async Task<ServiceResult> EditAsync(ImporterDto dto)
        {
            try
            {
                Persistence.Entities.OperationalCommercial.Importers importers = new()
                {
                    Key = dto.Key,
                    Name = dto.Name,
                    State = dto.State,
                    Identification = dto.Identification,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address,
                    countryId = dto.CountryId
                };
                bool existOtherImporters = (await _importersRepository.GetAllAsync())
                    .Any(i => i.Identification == importers.Identification && i.Identification != importers.Identification);
                if (existOtherImporters) return new ServiceResult { Success = false, Message = "Ya existe otro importador con esta identificacion", TypeAlert = "danger"};
                bool editar =  await _importersRepository.EditAsync(importers);
                if (editar) return new ServiceResult { Success = true,Message = "Importador editado con exito", TypeAlert = "success"};
                return new ServiceResult {Success = false, Message = "Ha ocurrido un error al editar el importador", TypeAlert = "danger"};
            }
            catch (Exception ex) {

                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion del servicio {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<IReadOnlyCollection<ImporterDto>> GetAllAsync()
        {
            try
            {
                var importers = await _importersRepository.GetAllAsync();
                var imp = new List<ImporterDto>();

                if (importers != null)
                {
                    foreach (var item in importers)
                    {
                        ImporterDto importerDto = new()
                        {
                            Key = item.Key,
                            Name = item.Name,
                            State = item.State,
                            Identification = item.Identification,
                            Phone = item.Phone,
                            Address = item.Address,
                            Email = item.Email,
                            CountryId = item.countryId,
                            CountryName = item.country!.Name

                        };
                        imp.Add(importerDto);

                    }
                    return imp;

                }return null!;
            }
            catch(Exception )
            {
                return null!;
            }
        }

        public async Task<ImporterDto> GetKeyAsync(int key)
        {
            try
            {
                var imp = await _importersRepository.GetEntityById(key);
                
                if(imp != null)
                {
                    ImporterDto importerDto = new()
                    {
                        Key = imp.Key,
                        Name = imp.Name,
                        State = imp.State,
                        Identification = imp.Identification,
                        Phone = imp.Phone,
                        Address = imp.Address,
                        Email = imp.Email,
                        CountryId = imp.countryId,
                        CountryName = imp.country!.Name

                    };

                    return importerDto;

                }return null!;

            }catch(Exception)
            {
                return null!;
            }
        }
    }
}
