using Application.DTOs.Suppliers;
using Application.Services.SuppliersServices;
using Application.ViewModel.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace ImportCost.Controllers.Suppliers
{
    public class SuppliersController : Controller
    {
        private readonly SuppliersServices _suppliersServices;

        public SuppliersController(SuppliersServices suppliersServices)
        {
            _suppliersServices = suppliersServices;
        }


        public async Task<IActionResult> Index()
        {
            var listS = new List<ViewModelSuppliers>();
            var listD = await _suppliersServices.GetAllAsync();

            foreach (var item in listD)
            {
                ViewModelSuppliers vS = new() { 
                        key = item.Key,
                        Name = item.Name,
                        CountryId = item.CountryId,
                        NameCountry = item.NameContry,
                        Email  =item.Email ?? "NA",
                        PhoneNumber =item.PhoneNumber ?? "-",
                        MainCurrency = item.CurrencyName,
                        MainCurrencyId = item.CurrencyId,
                        State = item.State
                };

                listS.Add(vS);
            }

            return View(listS);
        }

        //add metodo para rellenar de monedas y otro para rellenar de paises

        public async Task<IActionResult> Create() { 
            return View("Save", new ViewModelSuppliersSave {
                Name = "",
                Email = "",
                Phone = "",
                Countries = null!, //cambiar por el get country
                State = true,
                CountryId = 0, //cambiar por el id seleccionado
                Currencies = null!, //cambiar por el get currencies
                CurrencyId = 0 //cambiar por el id seleccioando
            });
        }
        public async Task<IActionResult> Edit(int key)
        {
            var s = await _suppliersServices.GetKeyAsync(key);
            if (s == null) return RedirectToRoute(new {controller ="Suppliers", action="Edit"});
            ViewModelSuppliersSave vs = new() { 
                Key = s.Key,
                Name = s.Name,
                CountryId = s.CountryId, //id seleccionado
                State = s.State,
                Email = s.Email  ?? "NA",
                Phone = s.PhoneNumber ?? "-",
                Countries = null!, //cambiar por el getCountries
                CurrencyId = s.CurrencyId, //id seleccionado
                Currencies = null!// cambiar por el getCurrencies
            };
            return View(vs);
        }
        public async Task<IActionResult> Delete(int key)
        {
            var s = await _suppliersServices.GetKeyAsync(key);
            if (s == null) return RedirectToRoute(new {controller = "Suppliers", action ="Delete"});
            return View("Delete", new ViewModelSuppliersDelete { Key = s.Key, NameSupplier = s.Name});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelSuppliersDelete vs)
        {
            if (!ModelState.IsValid) return RedirectToRoute(new {controller = "Suppliers", action  = "Delete"});
            var result = await _suppliersServices.DeleteAsync(vs.Key);
            if (!result.Success) return RedirectToRoute(new { controller = "Suppliers", action ="Delete"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new { controller = "Suppliers", action = "Index" });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelSuppliersSave vs)
        {
            if (!ModelState.IsValid) return RedirectToRoute(new {controller = "Suppliers", action ="Edit"});
            SuppliersDto sup = new() { 
                Key = vs.Key,
                Name = vs.Name,
                CountryId = vs.CountryId,
                Email = vs.Email,
                PhoneNumber = vs.Phone,
                State= vs.State,
                CurrencyId = vs.CurrencyId
            };
            var result = await _suppliersServices.EditAsync(sup);
            if (!result.Success) return RedirectToRoute(new { controller="Suppliers", action="Edit"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new {controller ="Suppliers",action="Index" });
        }
        public async Task<IActionResult> Create(ViewModelSuppliersSave vs)
        {
            if (!ModelState.IsValid) return RedirectToRoute(new { controller = "Suppliers", action = "Save" });
            SuppliersDto sp = new() { 
                Key = vs.Key,
                Name = vs.Name,
                State = vs.State,
                Email = vs.Email,
                PhoneNumber = vs.Phone,
                CountryId = vs.CountryId,
                CurrencyId = vs.CurrencyId
            };
            var result = await _suppliersServices.CreateAsync(sp);
            if (!result.Success) return RedirectToRoute(new {controller="Suppliers", action="Save"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new {controller ="Suppliers",  action="Index"});
        }
    }
}
