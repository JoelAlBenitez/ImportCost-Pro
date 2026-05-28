using Microsoft.AspNetCore.Mvc;
using Application.ViewModel.Products;
using Application.Services.ProductsServices;
using Application.Services.TarriffCategories;
using Application.ViewModel.Select;
using Application.DTOs.Products;

namespace ImportCost.Controllers.Products
{
    public class ProductsController : Controller
    {

        //agregar validaciones en el servicio que validen que si se agregar un largo... deben tener valores los tres
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
       
        private  async Task<List<ViewModelSelectCategories>> GetCategories()
        {
            var categories = await _tarriffCategoriesServices.GetAllAsync();
            var list = new List<ViewModelSelectCategories>();
            foreach (var item in categories!)
            {
                if (item.State)
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
       
        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelProductsSave vp)
        {
            if (!ModelState.IsValid) return View("Edit", vp);

            ProductsDto productsDto = new() {
                 Key = vp.Key,
                 Name = vp.Name,
                 State = vp.State,
                 CodeReference = vp.CodeReference,
                 TarriffCategoriesId = vp.CategoriesId,
                 UnitWeight = vp.UnitWeight,
                 Large = vp.Large,
                 Broad = vp.Broad,
                 High = vp.High,
                 Description = vp.Description,
                 unitMesaurement = vp.unit,
                 CountrysId = vp.CountryId
            };

            var result = await _productsServices.EditAsync(productsDto);
            if (!result.Success) return RedirectToRoute(new { controller = "Products", action = "Index" });
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return View("Edit", vp);
           
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelProductsDelete vp)
        {
            if (!ModelState.IsValid) return View("Delete", vp);
            var result = await _productsServices.DeleteAsync(vp.Key);
            if (!result.Success) return RedirectToRoute(new {controller = "Products", action = "Delete"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return new RedirectToRouteResult(new { controller = "Products", action = "Index" });
        }
      
        public async Task<IActionResult> Delete(int id)
        {
           var product = await _productsServices.GetKeyAsync(id);
            if(product  == null) return RedirectToRoute(new { controller = "Products", action ="Index" });
            return View("Delete", new ViewModelProductsDelete { Key = product.Key , Name = product.Name});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsServices.GetKeyAsync(id);
            if (product == null) return RedirectToRoute(new { controller = "Products", action = "Index" });
            var listCategories = await GetCategories();
            ViewModelProductsSave vp = new()
            {
                Key = product.Key,
                Name = product.Name,
                State = product.State,
                CodeReference = product.CodeReference,
                CategoriesId = product.TarriffCategoriesId,
                unit = product.unitMesaurement,
                UnitWeight = product.UnitWeight,
                Categories = listCategories,
                Large = product.Large,
                Broad = product.Broad,
                High = product.High,
                Description = product.Description,
                CountryId = product.CountrysId
                //agregar el elemento de paises cuando se descomente
            };
            return View("Edit", vp);
        }

        public async Task<IActionResult> Create()
        {
            var list = await GetCategories();
            //agregar categorias de paises cuando se bajen los cambios y unidad cambiar por listado.

            return View("Save", new ViewModelProductsSave { 
                    Key = 0,
                    Name = "",
                    State = true,
                    CodeReference = "",
                    UnitWeight = 0,
                    Categories = list,
                    Large = 0,
                    High = 0,
                    Broad = 0,
                    unit = Persistence.Entities.Enums.UnitMesaurement.Unit,
                    Description = "",
                    CategoriesId = "",
                    CountryId = 0,
            });
        }
    }
}
