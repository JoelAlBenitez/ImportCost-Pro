using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ExchangeRates;
using Application.Services.ExchangeRates;
using Application.Services.Currencies;
using Application.ViewModel.ExchangeRates;
using Application.ViewModel.Select;

namespace ImportCost.Controllers.ExchangeRates
{
    public class ExchangeRatesController : Controller
    {
        private readonly ExchangeRateService _exchangeRateService;
        private readonly CurrencyService _currencyService;

        public ExchangeRatesController(ExchangeRateService exchangeRateService, CurrencyService currencyService)
        {
            _exchangeRateService = exchangeRateService;
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _exchangeRateService.GetAllAsync();
            var listView = new List<ViewModelExchangeRate>();

            foreach (var item in list)
            {
                listView.Add(new ViewModelExchangeRate
                {
                    Key = item.Key,
                    SourceCurrencyName = item.SourceCurrencyName ?? "N/A",
                    DestinationCurrencyName = item.DestinationCurrencyName ?? "N/A",
                    RateValue = item.RateValue,
                    EffectiveDate = item.EffectiveDate,
                    State = item.State
                });
            }

            return View(listView);
        }

        private async Task<List<ViewModelSelectCurrency>> GetCurrencies()
        {
            var list = new List<ViewModelSelectCurrency>();
            var currencies = await _currencyService.GetAllAsync();
            foreach (var item in currencies)
            {
                if (item.State)
                {
                    list.Add(new ViewModelSelectCurrency
                    {
                        Id = item.Key,
                        NameCurrency = item.Name
                    });
                }
            }
            return list;
        }

        public async Task<IActionResult> Create()
        {
            return View("Save", new ViewModelExchangeRateSave
            {
                Key = 0,
                SourceCurrencyId = 0,
                DestinationCurrencyId = 0,
                RateValue = 0,
                EffectiveDate = DateTime.Now.Date,
                State = true,
                Currencies = await GetCurrencies()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelExchangeRateSave vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Currencies = await GetCurrencies();
                return View("Save", vm);
            }

            ExchangeRateDto dto = new()
            {
                Key = 0,
                SourceCurrencyId = vm.SourceCurrencyId,
                DestinationCurrencyId = vm.DestinationCurrencyId,
                RateValue = vm.RateValue,
                EffectiveDate = vm.EffectiveDate,
                State = vm.State
            };

            var result = await _exchangeRateService.CreateAsync(dto);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
                vm.Currencies = await GetCurrencies();
                return View("Save", vm);
            }

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rate = await _exchangeRateService.GetKeyAsync(id);

            if (rate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewModelExchangeRateSave vm = new()
            {
                Key = rate.Key,
                SourceCurrencyId = rate.SourceCurrencyId,
                DestinationCurrencyId = rate.DestinationCurrencyId,
                RateValue = rate.RateValue,
                EffectiveDate = rate.EffectiveDate,
                State = rate.State,
                Currencies = await GetCurrencies()
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelExchangeRateSave vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Currencies = await GetCurrencies();
                return View("Edit", vm);
            }

            ExchangeRateDto dto = new()
            {
                Key = vm.Key,
                SourceCurrencyId = vm.SourceCurrencyId,
                DestinationCurrencyId = vm.DestinationCurrencyId,
                RateValue = vm.RateValue,
                EffectiveDate = vm.EffectiveDate,
                State = vm.State
            };

            var result = await _exchangeRateService.EditAsync(dto);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
                vm.Currencies = await GetCurrencies();
                return View("Edit", vm);
            }

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int key)
        {
            var rate = await _exchangeRateService.GetKeyAsync(key);
            if (rate == null) return RedirectToAction(nameof(Index));

            return View("Delete", new ViewModelExchangeRateDelete
            {
                Key = rate.Key,
                Name = $"{rate.SourceCurrencyName} -> {rate.DestinationCurrencyName} ({rate.EffectiveDate.ToShortDateString()})"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelExchangeRateDelete vm)
        {
            var result = await _exchangeRateService.DeleteAsync(vm.Key);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                return RedirectToAction(nameof(Delete), new { key = vm.Key });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
