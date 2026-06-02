using Application.DTOs.Suppliers;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.SuppliersServices;
using Application.ViewModel.Select;
using Application.ViewModel.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace ImportCost.Controllers.Suppliers
{
    public class SuppliersController : Controller
    {
        private readonly SuppliersServices _suppliersServices;
        private readonly CountryService _countryService;
        private readonly CurrencyService _currencyService;

        public SuppliersController(SuppliersServices suppliersServices, CountryService countryService, CurrencyService currencyService)
        {
            _suppliersServices = suppliersServices;
            _countryService = countryService;
            _currencyService = currencyService;
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
                        NameCountry = item.NameContry!,
                        Email  = item.Email ?? "NA",
                        PhoneNumber =item.PhoneNumber ?? "-",
                        MainCurrency = item.CurrencyName!,
                        MainCurrencyId = item.CurrencyId,
                        State = item.State
                };

                listS.Add(vS);
            }

            return View(listS);
        }

        private async Task<List<ViewModelSelectCountries>> GetCountries(int key = 0)
        {
            var list = new List<ViewModelSelectCountries>();
            var countries = await _countryService.GetAllAsync();
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

        private async Task<List<ViewModelSelectCurrency>> GetCurrencies(int key =0)
        {
            var list = new List<ViewModelSelectCurrency>();
            var countries = await _currencyService.GetAllAsync();
            foreach (var item in countries)
            {
                if (item.State || key != 0 && item.Key != key)
                {
                    ViewModelSelectCurrency viewModelSelectCountries = new()
                    {
                        Id = item.Key,
                        NameCurrency = item.Name
                    };
                    list.Add(viewModelSelectCountries);
                }
            }
            return list;
        }

        public async Task<IActionResult> Create() { 

            return View("Save", new ViewModelSuppliersSave {
                Name = null!,
                Email = "",
                Phone = "",
                Countries = await GetCountries(), 
                State = true,
                CountryId = 0, 
                Currencies =  await GetCurrencies(),
                CurrencyId = 0 
            });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var s = await _suppliersServices.GetKeyAsync(id);
            if (s == null) { 
                RedirectToAction(nameof(Edit));
            }
            
            ViewModelSuppliersSave vs = new() { 
                Key = s!.Key,
                Name = s.Name,
                CountryId = s.CountryId, 
                State = s.State,
                Email = s.Email!,
                Phone = s.PhoneNumber!,
                Countries = await GetCountries(s.CountryId),
                CurrencyId = s.CurrencyId, 
                Currencies = await GetCurrencies(s.CurrencyId)
            };
            return View("Edit", vs);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _suppliersServices.GetKeyAsync(id);
            if (s == null) return RedirectToAction(nameof(Delete));
            return View("Delete", new ViewModelSuppliersDelete { Key = s.Key, NameSupplier = s.Name});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelSuppliersDelete vs)
        {
            if (!ModelState.IsValid) return View("Delete", vs);
              
            var result = await _suppliersServices.DeleteAsync(vs.Key);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelSuppliersSave vs)
        {
            if (!ModelState.IsValid) {

                vs.Currencies = await GetCurrencies();
                vs.Countries = await GetCountries();
                View("Edit", vs);
            }
            
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
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(ViewModelSuppliersSave vs)
        {
            if (!ModelState.IsValid) {
                vs.Currencies = await GetCurrencies();
                vs.Countries = await GetCountries();
                return View("Save", vs);
            }
            
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
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
    }
}
