using Application.Services.BaseServices;
using Application.Dto.TarriffCategories;
using Persistence.Repositories.OperationalCommercial;
using Persistence.Entities.OperationalCommercial;
namespace Application.Services.TarriffCategories
{
    public class TarriffCategoriesServices : IServicesBase<TariffCategoriesDto, string>
    {

        private readonly TariffCategoriesRepository  _tarriffCategoriesRepository;

        public TarriffCategoriesServices(TariffCategoriesRepository tarriffCategoriesRepository)
        {
            _tarriffCategoriesRepository = tarriffCategoriesRepository;
        }

        public async Task<bool> AssociateProductsByCategorie(string code)
        {
            try
            {
                return await _tarriffCategoriesRepository.AssociatedProductsC(code);
            }
            catch (Exception) {

                return false;
            }
        }
 
        public async Task<bool> CreateAsync(TariffCategoriesDto dto)
        {
            try
            {
                TariffCategories tariffCategories = new (){
                    Key = dto.Key.Trim(),
                    Name = dto.Name,
                    State = dto.State,
                    PorcentageTariff = dto.PorcentageTariff,
                    ITBIS = dto.ITBIS,
                    SelectiveTaxApplies = dto.SelectiveTaxApplies,
                    PorcentageTaxSelective = dto.PorcentageTaxSelective
                };
                return await _tarriffCategoriesRepository.CreateAsync(tariffCategories);  
            }
            catch (Exception) {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                return await _tarriffCategoriesRepository.DeleteAsync(key);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> EditAsync(TariffCategoriesDto dto)
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
                return await _tarriffCategoriesRepository.EditAsync(tariff);
            }
            catch (Exception)
            {
                return false;
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
