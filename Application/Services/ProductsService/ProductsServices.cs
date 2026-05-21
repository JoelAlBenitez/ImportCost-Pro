using Application.Dto.Products;
using Application.Services.BaseServices;
using Persistence.Context;
using Persistence.Entities.OperationalCommercial;
using Persistence.Repositories.OperationalCommercial;
using System.Security.AccessControl;

namespace Application.Services.ProductsServices
{
    public class ProductsServices : IServicesBase<ProductsDto, int>
    {

        private readonly ProductsRepository _productsRepository;

        public ProductsServices(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<bool> ExistProduct(ProductsDto dto)
        {
            try
            {
                return await _productsRepository.ExistProductsByCodeReference(dto.CodeReference);
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
                var exits = await ExistProduct(dto);
                if (exits) return false;
               
                Products products = new()
                {
                    Name = dto.Name,
                    State = dto.State,
                    CodeRefence = dto.CodeReference,
                    UnitWeight = dto.UnitWeight,
                    Large = dto.Large,
                    Broad = dto.Broad,
                    High = dto.High,
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
                    State = dto.State,
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
                            State = item.State,
                            CodeReference = item.CodeRefence,
                            UnitWeight = item.UnitWeight,
                            Large = item.Large,
                            Broad = item.Broad,
                            High = item.High,
                            Description = item.Description,
                            TarriffCategoriesId = item.tarrifCategoriesId ?? 0

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
                        Name = product.Name,
                        State = product.State,
                        CodeReference = product.CodeRefence,
                        UnitWeight = product.UnitWeight,
                        Large = product.Large,
                        Broad = product.Broad,
                        High = product.High,
                        TarriffCategoriesId = product.tarrifCategoriesId ?? 0,
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
