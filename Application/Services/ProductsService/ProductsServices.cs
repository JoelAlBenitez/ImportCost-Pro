using Application.Dto.Products;
using Application.Services.BaseServices;
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

        public async Task<bool> CreateAsync(ProductsDto dto)
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

                return await _productsRepository.CreateAsync(products);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int key)
        {
            try
            {
                return await _productsRepository.DeleteAsync(key);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditAsync(ProductsDto dto)
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
                return await _productsRepository.EditAsync(products);

            }
            catch (Exception)
            {
                return false;
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
