using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ExchangeRates;
using Application.Services.TaxConfigurations;

namespace ImportCost.Controllers
{
    public class HomeController : Controller
    {
        private readonly CountryService _countryService;
        private readonly CurrencyService _currencyService;
        private readonly ExchangeRateService _exchangeRateService;
        private readonly TaxConfigurationService _taxConfigurationService;

        public HomeController(
            CountryService countryService,
            CurrencyService currencyService,
            ExchangeRateService exchangeRateService,
            TaxConfigurationService taxConfigurationService)
        {
            _countryService = countryService;
            _currencyService = currencyService;
            _exchangeRateService = exchangeRateService;
            _taxConfigurationService = taxConfigurationService;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _countryService.GetAllAsync();
            var currencies = await _currencyService.GetAllAsync();
            var rates = await _exchangeRateService.GetAllAsync();
            var taxes = await _taxConfigurationService.GetAllAsync();

            ViewBag.TotalCountries = countries.Count;
            ViewBag.TotalCurrencies = currencies.Count;
            ViewBag.TotalRates = rates.Count;
            ViewBag.HasTaxConfig = taxes.Any();

            return View();
        }
     
    }
}
