using Microsoft.AspNetCore.Mvc;
using Application.ViewModel.Products;
using Application.Services.ProductsServices;
using Application.Services.TarriffCategories;
using Application.ViewModel.Select;
using Application.Dto.Products;

namespace ImportCost.Controllers.Products
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
       
        [HttpGet]
        private  async Task<List<ViewModelSelectCategories>> GetCategories()
        {
            var categories = await _tarriffCategoriesServices.GetAllAsync();
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
            return list;
        }

        //add controller consulta de paises y llenado de lista


        [HttpGet]
        private async Task<List<ViewModelSelectCountries>> GetCountries()
        {
            return null!;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelProductsSave vp)
        {
            if (!ModelState.IsValid) return View("Save", vp);

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

            //obtener listado de productos y validar que no existan mas productos con el codigo que se esta agregando

            var list = await _productsServices.GetAllAsync();
            int count = list.Count(x  => x.CodeReference == productsDto.CodeReference);
            if (count > 1) {
                TempData["Message"] = "No pueden existir multiples productos con el mismo codigo de referencia";
                TempData["TypeAlert"] = "danger";
                return RedirectToRoute(new { controller = "Products", action = "Index" });
            }

            var editP = await _productsServices.EditAsync(productsDto);
            if (editP)
            {
                TempData["Message"] = "Producto editado exitosamente";
                TempData["TypeAlert"] = "success";
                return RedirectToRoute(new {controller = "Products", action ="Index"} );
            }

            TempData["Message"] = "Ha ocurrido un error al editar el producto seleccionado, favor intente de nuevo";
            TempData["TypeAlert"] = "danger";
            return View("Save", vp);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ViewModelProductsSave vp)
        {
            if (!ModelState.IsValid) return View("Save", vp);

            ProductsDto products = new() { 
                Key =  0,
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
            //var extis = await _productsServices.ExistProduct(products.CodeReference);
            //if(extis)
            //{
            //    TempData["Message"] = "El codigo de referencia de este producto ya se encuentra registrado en el sistema";
            //    TempData["TypeAlert"] = "danger";
            //    return RedirectToRoute(new { controller = "Products", action = "Index" });
            //}

            //var appLargeHightBroad = vp.ValidateUnitMessaurent();
            //if (appLargeHightBroad == null)
            //{
            //    TempData["Message"] = appLargeHightBroad;
            //    TempData["TypeAlert"] = "danger";
            //    return RedirectToRoute(new { controller = "Products", action = "Index" }); 
            //}

            var create = await _productsServices.CreateAsync(products);
            if(create)
            {
                TempData["Message"] = "Producto registrado con exito";
                TempData["TypeAlert"] = "success";
                return RedirectToRoute(new {controller="Products", action = "Index"});
            }

            TempData["Message"] = "Ha ocurrido un error al intentar crear este nuevo producto, por favor intente de nuevo ";
            TempData["TypeAlert"] = "danger";
            return View("Sve", vp);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelProductsDelete vp)
        {
            if (!ModelState.IsValid) return View("Delete", vp);
            var delete = await _productsServices.DeleteAsync(vp.Key);

            //agregar logica para verificar si el producto esta asociado a ordenes de exportaciones antes de eliminarlo
            if (delete)
            {
                TempData["Message"] = "Producto eliminado con exito";
                TempData["TypeAlert"] = "success";
                return RedirectToRoute(new { controller = "Products", action = "Index" });
            }

            TempData["Message"] = "Ha ocurrido un error al eliminar el producto seleccionado, favor intente de nuevo";
            TempData["TypeAlert"] = "danger";
            return View("Delete", vp);
        }
        public async Task<IActionResult> Create()
        {
            return View("Save", new ViewModelProductsSave()
            {
                Name = "",
                State = true,
                CodeReference = "",
                UnitWeight = 0,
                Categories = await GetCategories(),
                Large = 0,
                Broad = 0,
                High = 0,
                unit = Persistence.Entities.Enums.UnitMesaurement.Unit, //mejorar enfoque para no usar directamente el enum de persistencia a pesar de 
                //herencia de referencia por la capa de application
                Description = "",
                CategoriesId = null!,
                CountryId = 0
            });

        }
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _productsServices.GetKeyAsync(id);
            if (p == null) return RedirectToRoute(new { controller = "Products", action = "Index" });

            var categories = GetCategories();
            if (categories != null) return RedirectToRoute(new { controller = "Products", action = "Index" });

            /*ViewModelProductsSave viewModel = new() { //descomentar cuando se agreguen los elementos de paises
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

            return RedirectToRoute(new { controller = "Products", action = "Index" });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var pro = await _productsServices.GetKeyAsync(id);
            if(pro == null)
            {
                TempData["Message"] = "Ha ocurrido un error al encontrar el producto seleccionado, favor intente de nuevo";
                TempData["TypeAlert"] = "danger";
                return RedirectToRoute(new {controller="Products", action ="Index"});
            }
            ViewModelProductsDelete vp = new() { Key = pro.Key, Name = pro.Name };
            return View(vp);
        }
       
    }
}
