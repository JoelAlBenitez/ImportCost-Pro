using Application.Dto.Importers;
using Application.Services.BaseServices;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.Importers
{
    public class ImportersServices : ServicesBase<ImporterDto, int>
    {
        private readonly ImportersRepository _importersRepository;
        public ImportersServices (ImportersRepository repository)
        {
            _importersRepository = repository;
        }

        public async Task<bool> CreateAsync(ImporterDto dto)
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
                    Address = dto.Address

                };
                return await _importersRepository.CreateAsync(importers);
                
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int key)
        {
            try
            {
                return await _importersRepository.DeleteAsync(key);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EditAsync(ImporterDto dto)
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
                    Address = dto.Address
                };

                return await _importersRepository.EditAsync(importers);

            }
            catch (Exception ex) {

                return false;
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
                            Email = item.Email

                        };
                        imp.Add(importerDto);

                    }
                    return imp;

                }return null!;


            }
            catch(Exception ex)
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
                        Email = imp.Email

                    };

                    return importerDto;

                }return null!;

            }catch(Exception ex)
            {
                return null!;
            }
        }
    }
}
