using Microsoft.AspNetCore.Mvc;
using Application.DTOs.TaxConfigurations;
using Application.Services.TaxConfigurations;
using Application.ViewModel.TaxConfigurations;

namespace ImportCost.Controllers.TaxConfigurations
{
    public class TaxConfigurationsController : Controller
    {
        private readonly TaxConfigurationService _taxConfigurationService;

        public TaxConfigurationsController(TaxConfigurationService taxConfigurationService)
        {
            _taxConfigurationService = taxConfigurationService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _taxConfigurationService.GetAllAsync();
            var listView = new List<ViewModelTaxConfiguration>();

            foreach (var item in list)
            {
                ViewModelTaxConfiguration viewModel = new()
                {
                    Key = item.Key,
                    GeneralItbisPercentage = item.GeneralItbisPercentage,
                    CustomsServiceRatePercentage = item.CustomsServiceRatePercentage,
                    State = item.State
                };
                listView.Add(viewModel);
            }

            return View(listView);
        }

        public IActionResult Create()
        {
            return View("Save", new ViewModelTaxConfigurationSave
            {
                Key = 0,
                GeneralItbisPercentage = 0,
                CustomsServiceRatePercentage = 0,
                State = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelTaxConfigurationSave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            TaxConfigurationDto dto = new()
            {
                Key = 0,
                GeneralItbisPercentage = vm.GeneralItbisPercentage,
                CustomsServiceRatePercentage = vm.CustomsServiceRatePercentage,
                State = vm.State
            };

            var result = await _taxConfigurationService.CreateAsync(dto);

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
            var taxConfig = await _taxConfigurationService.GetKeyAsync(id);

            if (taxConfig == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewModelTaxConfigurationSave vm = new()
            {
                Key = taxConfig.Key,
                GeneralItbisPercentage = taxConfig.GeneralItbisPercentage,
                CustomsServiceRatePercentage = taxConfig.CustomsServiceRatePercentage,
                State = taxConfig.State
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelTaxConfigurationSave vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            TaxConfigurationDto dto = new()
            {
                Key = vm.Key,
                GeneralItbisPercentage = vm.GeneralItbisPercentage,
                CustomsServiceRatePercentage = vm.CustomsServiceRatePercentage,
                State = vm.State
            };

            var result = await _taxConfigurationService.EditAsync(dto);

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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taxConfigurationService.DeleteAsync(id);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            return RedirectToAction(nameof(Index));
        }
    }
}
