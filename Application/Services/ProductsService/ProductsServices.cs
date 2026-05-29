using Application.DTOs.Products;
using Application.Services.BaseServices;
using Application.Services.Result;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.OperationalCommercial;

namespace Application.Services.ProductsServices
{
    public class ProductsServices : IServicesBase<ProductsDto, int>
    {

        private readonly ProductsRepository _productsRepository;

        public ProductsServices(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<bool> ExistProduct(string codeReference)
        {
            try
            {
                return await _productsRepository.ExistProductsByCodeReference(codeReference);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ServiceResult> CreateAsync(ProductsDto dto)
        {
            try
            {
            
                Products products = new()
                {
                    Name = dto.Name,
                    State = dto.State,
                    countryId = dto.CountrysId,
                    CodeRefence = dto.CodeReference,
                    UnitWeight = dto.UnitWeight,
                    Large = dto.Large,
                    Broad = dto.Broad,
                    High = dto.High,
                    Unit = dto.unitMesaurement,
                    Description = dto.Description,
                    tarrifCategoriesId = dto.TarriffCategoriesId
                };
                bool exit = await ExistProduct(dto.CodeReference);

                if (exit) return new ServiceResult { Success = false, Message = "Ya existe un producto con este codigo de referencia", TypeAlert = "danger" };
                bool create = await _productsRepository.CreateAsync(products);
                //agregar validacion de pais activo o no activo
                //agregar validacion de largo, ancho y algo por si uno de los tres tiene valores y los otros no
                if (create) return new ServiceResult { Success = false, Message = "Producto creado exitosamente", TypeAlert = "success" };
                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al crear el producto", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion con el servicio {ex.Message}", TypeAlert = "danger" };
            }
        }

        public async Task<ServiceResult> DeleteAsync(int key)
        {
            try
            {
                //agregar validacion de no eliminacion si el producto esta asociado a ordenes de importacion
                bool delete =  await _productsRepository.DeleteAsync(key);
                if (delete) return new ServiceResult { Success = true, Message = "Producto eliminado con exito", TypeAlert = "success" };
                return new ServiceResult { Success = false, Message = "Ha ocurrido un error al intentar eliminar el producto", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion del servicio {ex.Message}", TypeAlert = "danger"};
            }
        }

        public async Task<ServiceResult> EditAsync(ProductsDto dto)
        {
            try
            {
                Products products = new()
                {
                    Key = dto.Key,
                    Name = dto.Name,
                    countryId = dto.CountrysId,
                    State = dto.State,
                    Unit = dto.unitMesaurement,
                    CodeRefence = dto.CodeReference,
                    UnitWeight = dto.UnitWeight,
                    Large = dto.Large,
                    Broad = dto.Broad,
                    High = dto.High,
                    Description = dto.Description,
                    tarrifCategoriesId = dto.TarriffCategoriesId
                    
                };

                bool exitsMoreProductsWithSameCode = (await _productsRepository.GetAllAsync())
                        .Any(t => t.CodeRefence == products.CodeRefence && t.CodeRefence != products.CodeRefence);
                if (exitsMoreProductsWithSameCode) return new ServiceResult {Success = false, Message = "Ya existe otro producto asociado a este codigo de referencia" , TypeAlert = "danger"};
                    
                bool edit = await _productsRepository.EditAsync(products);

                if (edit) return new ServiceResult { Success = true, Message = "Producto editado exitosamente", TypeAlert = "success" };
                return new ServiceResult { Success = false, Message = "Ha ocurrido un error en al edicion del producto", TypeAlert = "danger" };

            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en al comunicacion del servicio {ex.Message}", TypeAlert = "danger"};
            }
        }

        public async Task<IReadOnlyCollection<ProductsDto>> GetAllAsync()
        {
            try
            {
                var productsList = new List<ProductsDto>();
                var products = await _productsRepository.GetAllAsync();

              
                if(products != null)
                {
                    foreach (var item in products)
                    {
                        ProductsDto productsDto = new()
                        {
                            Key = item.Key,
                            Name = item.Name,
                            CountrysId  = item.countryId,
                            CountryName = item.country!.Name,
                            unitMesaurement = item.Unit,
                            TariffCategoriesName = item.tariffCategories!.Name,
                            State = item.State,
                            CodeReference = item.CodeRefence,
                            UnitWeight = item.UnitWeight,
                            Large = item.Large,
                            Broad = item.Broad,
                            High = item.High,
                            Description = item.Description,
                            TarriffCategoriesId = item.tarrifCategoriesId 
                            

                        };
                        productsList.Add(productsDto);
                    }
                    return productsList;
                }

                return null!;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public async Task<ProductsDto> GetKeyAsync(int key)
        {
            try
            { 
                var product = await _productsRepository.GetEntityById(key);

                if(product != null)
                {
                    ProductsDto p = new() { 
                        Key = product.Key,
                        unitMesaurement = product.Unit,
                        Name = product.Name,
                        CountrysId = product.countryId,
                        CountryName = product.country!.Name,
                        State = product.State,
                        CodeReference = product.CodeRefence,
                        UnitWeight = product.UnitWeight,
                        Large = product.Large,
                        Broad = product.Broad,
                        High = product.High,
                        TarriffCategoriesId = product.tarrifCategoriesId,
                        TariffCategoriesName = product.tariffCategories!.Name,
                        Description = product.Description
                
                    };
                    return p;
                }
                return null!;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
