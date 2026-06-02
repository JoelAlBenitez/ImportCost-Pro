using Microsoft.AspNetCore.Mvc;
using Application.ViewModel.Products;
using Application.Services.ProductsServices;
using Application.Services.TarriffCategories;
using Application.ViewModel.Select;
using Application.DTOs.Products;
using Persistence.Entities.Enums;
using Application.Services.Countries;


namespace ImportCost.Controllers.Products
{
    public class ProductsController : Controller
    {

        private readonly ProductsServices _productsServices;
        private readonly TarriffCategoriesServices _tarriffCategoriesServices;
        private readonly CountryService _countriesService;

        public ProductsController(ProductsServices productsServices, TarriffCategoriesServices tarriffCategoriesServices, CountryService countriesService)
        {
            _productsServices = productsServices;
            _tarriffCategoriesServices = tarriffCategoriesServices;
            _countriesService = countriesService;
            
        }

        public async Task<IActionResult> Index()
        {
            var listProducts =  await _productsServices.GetAllAsync();
            var listViewProducts = new List<ViewModelProducts>();

            foreach (var item in listProducts)
            {
                ViewModelProducts p = new() {
                    key = item.Key,
                    Name = item.Name,
                    State = item.State,
                    CodeReference = item.CodeReference,
                    TariffCategoriesId = item.TarriffCategoriesId,
                    TarffCategoriesName = item.TariffCategoriesName!,
                    UnitWeight = item.UnitWeight,
                    Large = item.Large ?? 0,
                    Broad = item.Broad ?? 0,
                    High = item.High ?? 0,
                    unitMesaurement = item.unitMesaurement,
                    CountryId = item.CountrysId,
                    CountryName = item.CountryName!
                };
            
                listViewProducts.Add(p);

            }

            return View(listViewProducts);
        }

        private  async Task<List<ViewModelSelectCategories>> GetCategories(string? categorieId = null)
        {
            var categories = await _tarriffCategoriesServices.GetAllAsync();
            var list = new List<ViewModelSelectCategories>();
            foreach (var item in categories!)
            {
                if (item.State || (categorieId != null && categorieId == item.Key))
                {
                    ViewModelSelectCategories selectCategories = new()
                    {
                        CodeCategories = item.Key,
                        NameCategory = item.Name

                    };
                    list.Add(selectCategories);
                }
            }
            return list;
        }
      
        private List<ViewModelSelectUnit> GetUnitMeasurements()
        {
            return Enum.GetValues(typeof(UnitMeasurement))
                .Cast<UnitMeasurement>()
                .Select(e => new ViewModelSelectUnit
                { Id = (int)e, Name = e.ToString() })
                .ToList();
        }

        private async Task<List<ViewModelSelectCountries>> GetCountries(int key = 0)
        {
            var list = new List<ViewModelSelectCountries>();
            var countries = await _countriesService.GetAllAsync();
            foreach (var item in countries)
            {
                if (item.State || key != 0 && item.Key != key)
                {
                    ViewModelSelectCountries viewModelSelectCountries = new()
                    {
                        CountryId = item.Key,
                        CountryName = item.Name
                    };
                    list.Add(viewModelSelectCountries);
                }
            }
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelProductsSave vp)
        {
            if (!ModelState.IsValid)
            {

                vp.Categories = await GetCategories();
                vp.Units =  GetUnitMeasurements();
                vp.countries = await GetCountries();
                return View("Save", vp);
               
            }
            ProductsDto p = new()
            {
                Key = 0,
                Name = vp.Name,
                State = vp.State,
                CodeReference = vp.CodeReference,
                TarriffCategoriesId = vp.TariffCategoriesId,
                UnitWeight = vp.UnitWeight,
                Large = vp.Large ?? 0,
                Broad = vp.Broad ?? 0,
                High = vp.High ?? 0,
                Description = vp.Description,
                CountrysId = vp.CountryId,
                unitMesaurement = (UnitMesaurement)vp.unit
            };
            var result = await _productsServices.CreateAsync(p);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Create));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            var list = await GetCategories();
            
            return View("Save", new ViewModelProductsSave
            {
                Key = 0,
                Name = "",
                State = true,
                CodeReference = "",
                UnitWeight = 0,
                Categories = list,
                Large = 0,
                High = 0,
                Broad = 0,
                unit = 0,
                Description = "",
                TariffCategoriesId = "",
                CountryId = 0,
                Units = GetUnitMeasurements(),
                countries = await GetCountries()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelProductsSave vp)
        {
            if (!ModelState.IsValid)
            {
                vp.Categories = await GetCategories(vp.TariffCategoriesId);
                vp.Units = GetUnitMeasurements();
                vp.countries = await GetCountries(vp.CountryId);
                return View("Edit", vp);
            }

            ProductsDto productsDto = new() {
                 Key = vp.Key,
                 Name = vp.Name,
                 State = vp.State,
                 CodeReference = vp.CodeReference,
                 TarriffCategoriesId = vp.TariffCategoriesId,
                 UnitWeight = vp.UnitWeight,
                 Large = vp.Large,
                 Broad = vp.Broad,
                 High = vp.High,
                 Description = vp.Description,
                 unitMesaurement = (UnitMesaurement)vp.unit,
                 CountrysId = vp.CountryId
            };

            var result = await _productsServices.EditAsync(productsDto);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Index)); ;
            return RedirectToAction(nameof(Index));
           
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsServices.GetKeyAsync(id);
            if (product == null) return RedirectToAction(nameof(Index)); ;
            var listCategories = await GetCategories(product.TarriffCategoriesId);
            ViewModelProductsSave vp = new()
            {
                Key = product.Key,
                Name = product.Name,
                State = product.State,
                CodeReference = product.CodeReference,
                TariffCategoriesId = product.TarriffCategoriesId,
                unit = ((int)product.unitMesaurement),
                UnitWeight = product.UnitWeight,
                Categories = listCategories,
                Large = product.Large,
                Broad = product.Broad,
                High = product.High,
                Description = product.Description,
                CountryId = product.CountrysId,
                Units = GetUnitMeasurements(),
                countries = await GetCountries()

            };
            return View("Edit", vp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelProductsDelete vp)
        {
            if (!ModelState.IsValid)  {
                return View("Delete", vp);
            }

            var result = await _productsServices.DeleteAsync(vp.Key);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Index)); ;
            return   RedirectToAction(nameof(Index)); ;
        }
      
        public async Task<IActionResult> Delete(int id)
        {
           var product = await _productsServices.GetKeyAsync(id);
            if(product  == null) return RedirectToRoute(new { controller = "Products", action ="Index" });
            return View("Delete", new ViewModelProductsDelete { Key = product.Key , Name = product.Name});
        }

      
    }
}
