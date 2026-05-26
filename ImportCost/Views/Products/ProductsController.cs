using Microsoft.AspNetCore.Mvc;
using Application.ViewModel.Products;
using Application.Services.ProductsServices;
using Application.Services.TarriffCategories;
using Application.ViewModel.Select;

namespace ImportCost.Views.Products
{
    public class ProductsController : Controller
    {

        private readonly ProductsServices _productsServices;
        private readonly TarriffCategoriesServices _tarriffCategoriesServices;

        public ProductsController(ProductsServices productsServices, TarriffCategoriesServices tarriffCategoriesServices)
        {
            _productsServices = productsServices;
            _tarriffCategoriesServices = tarriffCategoriesServices;
            //note: add services countries to load the countries. 
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
                    TarffCategoriesName = item.TariffCategoriesName,
                    UnitWeight = item.UnitWeight,
                    Large = item.Large ?? 0,
                    Broad = item.Broad ?? 0,
                    High = item.High ?? 0,
                    unitMesaurement = item.unitMesaurement,
                    CountryId = item.CountrysId,
                    CountryName = item.CountryName
                };        
            }
            return View(listViewProducts);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var p = await _productsServices.GetKeyAsync(id);
            if (p == null) return RedirectToRoute(new { controller = "Products", action = "Index"});

            var categories = await _tarriffCategoriesServices.GetAllAsync();
            if (categories != null) return RedirectToRoute(new {controller = "Products", action = "Index"});

            var list = new List<ViewModelSelectCategories>();
            foreach (var item in categories!)
            {
                ViewModelSelectCategories selectCategories = new()
                {
                    CodeCategories = item.Key,
                    NameCategory = item.Name

                };
                list.Add(selectCategories);
            }

            /*ViewModelProductsSave viewModel = new() {
                Name = p.Name,
                CodeReference = p.CodeReference,
                UnitWeight = p.UnitWeight,
                Categories = list,
                Large = p.Large ?? 0,
                Broad = p.Broad ?? 0,
                High = p.High ?? 0,
                unit = p.unitMesaurement,
                countries = listCountries,
                Description = p.Description,
                State = p.state

            };*/

            return View();
        }

        //add controller consulta de categorias y llenado de lista

        //add controller consulta de paises y llenado de lista



    }
}
