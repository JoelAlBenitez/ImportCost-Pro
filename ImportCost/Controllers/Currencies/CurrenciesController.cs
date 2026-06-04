using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Currencies;
using Application.Services.Currencies;
using Application.ViewModel.Currencies;

namespace ImportCost.Controllers.Currencies
{
    public class CurrenciesController : Controller
    {
        private readonly CurrencyService _currencyService;

        public CurrenciesController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _currencyService.GetAllAsync();
            var listView = new List<ViewModelCurrency>();

            foreach (var item in list)
            {
                listView.Add(new ViewModelCurrency
                {
                    key = item.Key,
                    Name = item.Name,
                    IsoCode = item.IsoCode,
                    Symbol = item.Symbol,
                    IsLocalCurrency = item.IsLocalCurrency,
                    State = item.State
                });
            }

            return View(listView);
        }

        public IActionResult Create()
        {
            return View("Save", new ViewModelCurrencySave
            {
                Key = 0,
                Name = "",
                IsoCode = "",
                Symbol = "",
                IsLocalCurrency = false,
                State = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelCurrencySave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            CurrencyDto dto = new()
            {
                Key = 0,
                Name = vm.Name,
                IsoCode = vm.IsoCode,
                Symbol = vm.Symbol,
                IsLocalCurrency = vm.IsLocalCurrency,
                State = vm.State
            };

            var result = await _currencyService.CreateAsync(dto);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                return View("Save", vm);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currency = await _currencyService.GetKeyAsync(id);

            if (currency == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewModelCurrencySave vm = new()
            {
                Key = currency.Key,
                Name = currency.Name,
                IsoCode = currency.IsoCode,
                Symbol = currency.Symbol,
                IsLocalCurrency = currency.IsLocalCurrency,
                State = currency.State
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelCurrencySave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            CurrencyDto dto = new()
            {
                Key = vm.Key,
                Name = vm.Name,
                IsoCode = vm.IsoCode,
                Symbol = vm.Symbol,
                IsLocalCurrency = vm.IsLocalCurrency,
                State = vm.State
            };

            var result = await _currencyService.EditAsync(dto);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                return View("Edit", vm);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int key)
        {
            var currency = await _currencyService.GetKeyAsync(key);
            if (currency == null) return RedirectToAction(nameof(Index));

            return View("Delete", new ViewModelCurrencyDelete
            {
                Key = currency.Key,
                Name = currency.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelCurrencyDelete vm)
        {
            var result = await _currencyService.DeleteAsync(vm.Key);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            return RedirectToAction(nameof(Index));
        }
    }
}
