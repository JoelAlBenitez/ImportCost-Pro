using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Countries;
using Application.Services.Countries;
using Application.ViewModel.Countries;

namespace ImportCost.Controllers.Countries
{
    public class CountriesController : Controller
    {
        private readonly CountryService _countryService;

        public CountriesController(CountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _countryService.GetAllAsync();
            var listView = new List<ViewModelCountry>();

            foreach (var item in list)
            {
                listView.Add(new ViewModelCountry
                {
                    Key = item.Key,
                    Name = item.Name,
                    IsoCode = item.IsoCode,
                    State = item.State
                });
            }

            return View(listView);
        }

        public IActionResult Create()
        {
            return View("Save", new ViewModelCountrySave
            {
                Key = 0,
                Name = "",
                IsoCode = "",
                State = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelCountrySave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            CountryDto dto = new()
            {
                Key = 0,
                Name = vm.Name,
                IsoCode = vm.IsoCode,
                State = vm.State
            };

            var result = await _countryService.CreateAsync(dto);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
                return View("Save", vm);
            }

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var country = await _countryService.GetKeyAsync(id);

            if (country == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewModelCountrySave vm = new()
            {
                Key = country.Key,
                Name = country.Name,
                IsoCode = country.IsoCode,
                State = country.State
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelCountrySave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            CountryDto dto = new()
            {
                Key = vm.Key,
                Name = vm.Name,
                IsoCode = vm.IsoCode,
                State = vm.State
            };

            var result = await _countryService.EditAsync(dto);

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
                return View("Edit", vm);
            }

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int key)
        {
            var country = await _countryService.GetKeyAsync(key);
            if (country == null) return RedirectToAction(nameof(Index));

            return View("Delete", new ViewModelCountryDelete
            {
                Key = country.Key,
                Name = country.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelCountryDelete vm)
        {
            var result = await _countryService.DeleteAsync(vm.Key);

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
