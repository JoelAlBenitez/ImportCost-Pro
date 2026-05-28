using Application.DTOs.Suppliers;
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

                var valid = await ValidateExistOtherSupplierWithData(dto);
                if (valid != null) return valid; 
               
                bool create = await _suppliersRepository.CreateAsync(s);
                if (create) return new ServiceResult {Success= true, Message = "Suplidor creado con exito", TypeAlert ="success"};
                return new ServiceResult{ Success = true, Message = "Ha ocurrido un error al intentar crear el suplidor", TypeAlert = "danger"};
                
            }catch(Exception ex)
            {
                return new ServiceResult {Success = true, Message = $"Ha ocurrido un error en la comunicación con el servicio {ex.Message}", TypeAlert= "danger"};
            }
        }

        public async Task<ServiceResult> DeleteAsync(int key)
        {
            try {


                bool AssociateImportationOrderBySuppliers = await _suppliersRepository.AssociateImportationOrderBySupplier(key);
                if(AssociateImportationOrderBySuppliers) return new ServiceResult
                {
                    Success = false,
                    Message = "Este suplidor tiene ordenes asociadas por lo que no se puede eliminar, se quiere evitar su uso se recomienda editar su estado en el módulo de edición ",
                    TypeAlert = "danger"
                };

                bool delete = await _suppliersRepository.DeleteAsync(key);
                if (delete) return new ServiceResult{Success = true, Message = "Suplidor eliminado con exito", TypeAlert ="success"};
                return new ServiceResult { Success=false, Message ="Ha ocurrido  un error al intentar eliminar al suplidor", TypeAlert="danger" };

            }catch(Exception ex)
            {

                return new ServiceResult { Success = true, Message = $"Ha ocurrido un error en la comunicación con el servicio {ex.Message}", TypeAlert = "danger" };
            }
        }

        private async Task<ServiceResult> ValidateExistOtherSupplierWithData(SuppliersDto s)
        {
            bool exitsOtherSupplierName = (await _suppliersRepository.GetAllAsync())
                        .Any(su => su.Name.Trim() == s.Name.Trim() && su.Name.Trim() != s.Name.Trim());
            if (exitsOtherSupplierName) return new ServiceResult { Success = false, Message = "Ya existe otro suplidor con este nombre, favor verificar este dato", TypeAlert = "danger" };

            bool exitsOtherSupplierEmail = (await _suppliersRepository.GetAllAsync())
                       .Any(su => su.Email!.Trim() == s.Email!.Trim() && su.Email.Trim() != s.Email.Trim());
            if (exitsOtherSupplierName) return new ServiceResult { Success = false, Message = "Ya existe otro suplidor con este email, favor verificar este dato", TypeAlert = "danger" };

            bool exitsOtherSupplierPhone = (await _suppliersRepository.GetAllAsync())
                       .Any(su => su.Phone!.Trim() == s.PhoneNumber!.Trim() && su.Phone!.Trim() != s.PhoneNumber!.Trim());
            if (exitsOtherSupplierName) return new ServiceResult { Success = false, Message = "Ya existe otro suplidor con este número telefonico, favor verificar este dato", TypeAlert = "danger" };

            return null!;
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

                var valid = await ValidateExistOtherSupplierWithData(dto);
                if (valid != null) return  valid;
                bool AssociateImportationOrderBySuppliers = await _suppliersRepository.AssociateImportationOrderBySupplier(s.Key);
                var supplier = await _suppliersRepository.GetEntityById(s.Key);
                if (AssociateImportationOrderBySuppliers)
                {
                    bool country = supplier.countryId != s.countryId;
                    bool currency = supplier.MainCurrencyId != s.MainCurrencyId;
                    if (country || currency) return new ServiceResult {Success = false, Message="Este suplidor tiene ordenes asociadas por " +
                        "lo que no puede modificar su pais de origen o moneda principal, si quiere evitar el uso de este puede editar su estado", TypeAlert = "danger" }; 
                }
                bool edit = await _suppliersRepository.EditAsync(s);
                if (edit) return new ServiceResult { Success = true,Message ="Suplidor editado con exito", TypeAlert ="success"};
                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al intentar editar el suplidor", TypeAlert = "danger" };

            }
            catch (Exception ex) { return new ServiceResult {Success = false, Message = $"Ha ocurrido en la comunicación con el servicio {ex.Message}", TypeAlert="danger"}; }
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
