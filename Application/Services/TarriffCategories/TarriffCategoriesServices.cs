using Application.Services.BaseServices;
using Persistence.Repositories.OperationalCommercial;
using Persistence.Entities.OperationalCommercial;
using Application.Services.Result;
using Application.DTOs.TarriffCategories;
namespace Application.Services.TarriffCategories
{
    public class TarriffCategoriesServices : IServicesBase<TariffCategoriesDto, string>
    {

        private readonly TariffCategoriesRepository  _tarriffCategoriesRepository;

        public TarriffCategoriesServices(TariffCategoriesRepository tarriffCategoriesRepository)
        {
            _tarriffCategoriesRepository = tarriffCategoriesRepository;
        }

        public async Task<bool> CategorieHasAssociateProducts(string code)
        {
            try
            {
                return await _tarriffCategoriesRepository.AssociatedProductsC(code);
            }
            catch (Exception) {

                return false;
            }
        }
 
        public async Task<ServiceResult> CreateAsync(TariffCategoriesDto dto)
        {
            try
            {
                var t = await GetKeyAsync(dto.Key);
                if (t != null) return new ServiceResult() {Success = false, Message =" Ya existe una categoria arancelaria con este codigo", TypeAlert = "danger"};
                TariffCategories tariffCategories = new (){
                    Key = dto.Key.Trim(),
                    Name = dto.Name,
                    State = dto.State,
                    PorcentageTariff = dto.PorcentageTariff,
                    ITBIS = dto.ITBIS,
                    SelectiveTaxApplies = dto.SelectiveTaxApplies,
                    PorcentageTaxSelective = dto.PorcentageTaxSelective
                };

                bool create =  await _tarriffCategoriesRepository.CreateAsync(tariffCategories);
                if (create) return new ServiceResult { Success = true, Message = "Categoria creada con exito", TypeAlert = "success" };
                 
               return new ServiceResult { Success = false, Message = "Ha ocurrido un error en la creacion de la categoria", TypeAlert = "danger" };
            }
            catch (Exception ex) {
                return new ServiceResult { Success = false, Message = $"Ha ocurrido un error en la comunicacion del servicio {ex.Message}", TypeAlert = "danger"};
            }
        }

        public async Task<ServiceResult> DeleteAsync(string key)
        {
            try
            {
                bool categoriesHasProducts = await CategorieHasAssociateProducts(key);
                if (categoriesHasProducts) return new ServiceResult()
                {
                    Success = false,
                    Message = 
                    "Esta categoria tiene productos asociados por lo que no se puede eliminar, pero puede editar su estado en el apartado de edicion", 
                    TypeAlert = "danger"
                };

                bool delete = await _tarriffCategoriesRepository.DeleteAsync(key);
                if (delete) return new ServiceResult() { Success = true, Message = "Categoria arancelaria eliminada con exito", TypeAlert = "success"};
                return new ServiceResult() { Success = false, Message = " Ha ocurrido un error en la eliminacion de la categoria", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult() { Success = false, Message = $"Ha ocurrido un error en la comunicacion del servicio {ex.Message}", TypeAlert = "danger"} ;
            }
        }
        public async Task<ServiceResult> EditAsync(TariffCategoriesDto dto)
        {
            try
            {
                TariffCategories tariff = new() {
                    Key = dto.Key.Trim(),
                    Name = dto.Name,
                    State = dto.State,
                    PorcentageTariff = dto.PorcentageTariff,
                    ITBIS = dto.ITBIS,
                    SelectiveTaxApplies = dto.SelectiveTaxApplies,
                    PorcentageTaxSelective = dto.PorcentageTaxSelective  
                };

                bool tarrP = await _tarriffCategoriesRepository.AssociatedProductsC(tariff.Key);
                var tarf = await _tarriffCategoriesRepository.GetEntityById(tariff.Key);
                if(tarrP)
                {
                    bool modifieCode = dto.Key != tarf.Key;
                    bool modifiePorcentage = dto.PorcentageTariff != tarf.PorcentageTariff;
                    bool modifieITBIS = dto.ITBIS != tarf.ITBIS;
                    bool modifieSelectiveTax = dto.SelectiveTaxApplies != tarf.SelectiveTaxApplies;
                    bool modifiePorcetageSelectiveTaz = dto.PorcentageTaxSelective != tarf.PorcentageTaxSelective;

                    if (modifieCode || modifieITBIS || modifiePorcentage || modifiePorcetageSelectiveTaz
                        || modifieSelectiveTax
                        ) return new ServiceResult
                        {
                            Success = false,
                            Message = "The category has associated products, so the ITBIS, Category Reference Code, " +
                            "Tariff Percentage, whether selective tax applies, or the selective tax percentage cannot be modified.",
                            TypeAlert = "danger"

                        };
         
                }
                
                bool edit = await _tarriffCategoriesRepository.EditAsync(tariff);
                if (edit) return new ServiceResult { Success = true, Message = "Categoria arancelaria modificada con extio", TypeAlert = "success" };
                return new ServiceResult() { Success = false, Message = "Ha ocurrido un error al procesar la modificacion", TypeAlert = "danger" };
            }
            catch (Exception ex)
            {
                return new ServiceResult() { Success = false, Message = $"Error en la comunicacion con el servicio {ex.Message} ", TypeAlert = "danger" };
            }
        }

        public async Task<IReadOnlyCollection<TariffCategoriesDto>> GetAllAsync()
        {
            try
            {
                var tarrifList = new List<TariffCategoriesDto>();
                var tarriffs = await _tarriffCategoriesRepository.GetAllAsync();


                if (tarriffs.Any())
                {
                    foreach (var item in tarriffs)
                    {
                        TariffCategoriesDto tariff = new()
                        {
                            Key = item.Key!,
                            Name = item.Name,
                            State = item.State,
                            PorcentageTariff = item.PorcentageTariff,
                            ITBIS = item.ITBIS,
                            SelectiveTaxApplies = item.SelectiveTaxApplies,
                            PorcentageTaxSelective = item.PorcentageTaxSelective

                        };
                        tarrifList.Add(tariff);
                    }
                    return tarrifList;
                }

                return null!;

            }catch(Exception) { return null!; }
        }
        public async Task<TariffCategoriesDto> GetKeyAsync(string key)
        {
            try
            {
                var tarrifR = await _tarriffCategoriesRepository.GetEntityById(key);
                if(tarrifR != null)
                {
                    TariffCategoriesDto tariff = new() { 
                        Key = tarrifR.Key!,
                        Name = tarrifR.Name,
                        State = tarrifR.State,
                        PorcentageTariff = tarrifR.PorcentageTariff,
                        ITBIS = tarrifR.ITBIS,
                        SelectiveTaxApplies = tarrifR.SelectiveTaxApplies,
                        PorcentageTaxSelective = tarrifR.PorcentageTaxSelective
                    };
                    return tariff;

                }return null!;

            }catch(Exception) { return null!; }
        }
    }
}
