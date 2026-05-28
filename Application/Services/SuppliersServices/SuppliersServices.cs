using Application.Dto.Suppliers;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.SuppliersServices
{
    public class SuppliersServices : IServicesBase<SuppliersDto, int>
    {

        private readonly SuppliersRepository  _suppliersRepository;

        public  SuppliersServices(SuppliersRepository suppliersRepository)
        {
            _suppliersRepository = suppliersRepository;
        }

        public async Task<ServiceResult> CreateAsync(SuppliersDto dto)
        {
            try
            {
                Suppliers s = new Suppliers { 
                     Name = dto.Name,
                     countryId = dto.CountryId,
                     Email = dto.Email,
                     State = dto.State,
                     Phone= dto.PhoneNumber,
                     MainCurrencyId = dto.CurrencyId
                };

                bool exits =  await _suppliersRepository.ExistName(s.Name.Trim());
                //agregar validacion de email no duplicado o asociado al usuario que se esta intentado crear
                if (exits) return new ServiceResult {Success =false,Message= "Ya existe un suplidor con este nombre", TypeAlert ="danger"};
                bool create = await _suppliersRepository.CreateAsync(s);
                if (create) return new ServiceResult {Success= true, Message = "Suplidor creado con exito", TypeAlert ="success"};
                return new ServiceResult{ Success = true, Message = "Ha ocurrido un error al intentar crear el suplidor", TypeAlert = "danger"};
                
            }catch(Exception ex)
            {
                return new ServiceResult {Success = true, Message = $"Ha ocurrido un error en la comunicacion con el servicio {ex.Message}", TypeAlert= "danger"};
            }
        }

        public async Task<ServiceResult> DeleteAsync(int key)
        {
            try {


                //aregar validaciones de ordenes de importancion 

                bool delete = await _suppliersRepository.DeleteAsync(key);
                if (delete) return new ServiceResult{Success = true, Message = "Suplidor eliminado con exito", TypeAlert ="success"};
                return new ServiceResult { Success=false, Message ="Ha ocurrido  un error al intentar eliminar al suplidor", TypeAlert="danger" };

            }catch(Exception ex)
            {

                return new ServiceResult { Success = true, Message = $"Ha ocurrido un error en la comunicacion con el servicio {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> EditAsync(SuppliersDto dto)
        {
            try
            {

                Suppliers s = new Suppliers
                {
                    Key = dto.Key,
                    Name = dto.Name,
                    countryId = dto.CountryId,
                    Email = dto.Email,
                    State = dto.State,
                    Phone = dto.PhoneNumber,
                    MainCurrencyId = dto.CurrencyId
                };

               
                bool exitsOtherSupplier = (await _suppliersRepository.GetAllAsync()) //agregar validacion de asociar datos criticos como phone,email
                                                                                     //o adress con suplidores ya registrados
                        .Any(su => su.Name.Trim() == s.Name.Trim() && su.Name.Trim() != s.Name.Trim());
               
                if (exitsOtherSupplier) return new ServiceResult{Success = false, Message="Ya existe otro suplidor con este nombre, favor verificar este dato", TypeAlert ="danger"};
                //agregar validacion de si el producto tiene ordenes de importancion -> campos criticos
                bool edit = await _suppliersRepository.EditAsync(s);
                if (edit) return new ServiceResult { Success = true,Message ="Suplidor editado con exito", TypeAlert ="success"};
                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al intentar editar el suplidor", TypeAlert = "danger" };

            }
            catch (Exception ex) { return new ServiceResult {Success = false, Message = $"Ha ocurrido en la comunicacion con el servicio {ex.Message}", TypeAlert="danger"}; }
        }

        public async Task<IReadOnlyCollection<SuppliersDto>> GetAllAsync()
        {
            try
            {
                var list =  await _suppliersRepository.GetAllAsync();
                var listS = new List<SuppliersDto>();
                foreach (var item in list)
                {
                    SuppliersDto s = new()
                    {
                        Key = item.Key,
                        Name = item.Name,
                        State = item.State,
                        NameContry = item.Country!.Name,
                        CountryId = item.countryId,
                        Email = item.Email ?? "NA",
                        PhoneNumber = item.Phone ?? "-",
                        CurrencyId = item.MainCurrencyId,
                        CurrencyName = item.MainCurrency!.Name

                    };
                    listS.Add(s);
                }
                return listS;
            } catch (Exception) { return null!; }

        }

        public  async Task<SuppliersDto> GetKeyAsync(int key)
        {
            try
            {
                var supplier = await _suppliersRepository.GetEntityById(key);
                SuppliersDto s = new() { 
                    Key = supplier.Key,
                    Name = supplier.Name,
                    State = supplier.State,
                    NameContry = supplier.Country!.Name,
                    CountryId = supplier.countryId,
                    Email = supplier.Email ?? "NA",
                    PhoneNumber = supplier.Phone ?? "-",
                    CurrencyId = supplier.MainCurrencyId,
                    CurrencyName =supplier.MainCurrency!.Name
                  
                };
                return s;
            }
            catch (Exception)
            {
                return null!; 
            }
        }
    }
}
