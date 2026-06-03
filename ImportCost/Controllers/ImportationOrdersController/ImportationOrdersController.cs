using Application.DTOs.Orders;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ImportationOrderServices;
using Application.Services.Importers;
using Application.Services.SuppliersServices;
using ImportCost.ViewModels.ImportationOrders;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities.Enums;

namespace ImportCost.Controllers.ImportationOrdersController
{
    public class ImportationOrdersController : Controller
    {
        // DEPENDENCIAS
        private readonly ImportationOrderService _orderService;
        private readonly ImportersServices _importersService;
        private readonly CountryService _countryService;

        private readonly SuppliersServices _suppliersService;
        private readonly CurrencyService _currenciesService;

        // CONSTRUCTOR CON INYECCIÓN MÚLTIPLE
        public ImportationOrdersController(
            ImportationOrderService orderService,
            ImportersServices importersService,
            CountryService countryService,
            SuppliersServices suppliersService,
            CurrencyService currenciesService)
        {
            _orderService = orderService;
            _importersService = importersService;
            _countryService = countryService;
            _suppliersService = suppliersService;
            _currenciesService = currenciesService;
        }
        //Action para mostrar la vista de creación de orden de importación

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();

            var viewModelList = orders.Select(order => new ImportationOrderViewModel
            {
                OrderId = order.OrderId,

                ImporterName = order.ImporterName ?? "Desconocido",
                SupplierName = order.SupplierName ?? "Desconocido",
                OriginCountryName = order.OriginCountryName ?? "Desconocido",
                CurrencyCode = order.CurrencyCode ?? "N/A",

                OrderDate = order.OrderDate,
                TransportMode = order.TransportMode.ToString(),
                OrderState = order.OrderState.ToString(),

                TotalFob = order.TotalFOB,
                EstimatedTotalCost = order.TotalImportationExpected
            }).ToList();

            return View(viewModelList);
        }
            [HttpPost]
            public async Task<IActionResult> Create(ImportationOrderCreateViewModel viewModel)
            {
                
                if (!ModelState.IsValid)
                {
                    await LoadCatalogsAsync(viewModel);
                    return View(viewModel);
                }

                
                var createDto = new ImportationOrderCreateDTO
                {
                    OrderId = viewModel.OrderId,
                    ImporterId = viewModel.ImporterId,
                    SupplierId = viewModel.SupplierId,
                    OriginCountryId = viewModel.OriginCountryId,
                    CurrencyId = viewModel.CurrencyId,
                    OrderDate = viewModel.OrderDate,
                    TransportMode = viewModel.TransportMode
                };


            var result = await _orderService.CreateOrderAsync(createDto);

            TempData["Message"] = result.Message;
            TempData["TypeMessage"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadCatalogsAsync(viewModel);
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCatalogsAsync(dynamic viewModel,
            int? currentImporterId = null,
            int? currentCountryId = null,
            int? currentSupplierId = null,
            int? currentCurrencyId = null)
        {
            var importers = await _importersService.GetAllAsync();
            var countries = await _countryService.GetAllAsync();
            var suppliers = await _suppliersService.GetAllAsync();
            var currencies = await _currenciesService.GetAllAsync();

            viewModel.ImportersList = importers
                .Where(i => i.State == true || i.Key == currentImporterId)
                .Select(i => new Application.ViewModel.Select.ViewModelSelectImporters
                {
                    ImporterId = i.Key,
                    ImporterName = i.Name
                }).ToList();

            viewModel.CountriesList = countries
                .Where(c => c.State == true || c.Key == currentCountryId)
                .Select(c => new Application.ViewModel.Select.ViewModelSelectCountries
                {
                    CountryId = c.Key,
                    CountryName = c.Name
                }).ToList();

            viewModel.SuppliersList = suppliers
                .Where(s => s.State == true || s.Key == currentSupplierId)
                .Select(s => new Application.ViewModel.Select.ViewModelSelectSuppliers
                {
                    SupplierId = s.Key,
                    SupplierName = s.Name
                }).ToList();

            viewModel.CurrenciesList = currencies
                .Where(c => c.State == true || c.Key == currentCurrencyId)
                .Select(c => new Application.ViewModel.Select.ViewModelSelectCurrency
                {
                    Id = c.Key,
                    NameCurrency = c.Name
                }).ToList();
        }

        [HttpGet]
        

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var order = await _orderService.GetEntityById(id);
            if (order == null)
            {
                TempData["Message"] = "No se encontró la orden.";
                TempData["TypeMessage"] = "error";
                return RedirectToAction(nameof(Index));
            }

            if (order.OrderState == OrderState.Cerrada || order.OrderState == OrderState.Cancelada)
            {
                TempData["Message"] = "No se puede editar esta orden porque está cerrada o cancelada.";
                TempData["TypeMessage"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ImportationOrderEditViewModel
            {
                OrderId = order.OrderId,
                ImporterId = order.ImporterId,
                SupplierId = order.SupplierId,
                OriginCountryId = order.OriginCountryId,
                CurrencyId = order.CurrencyId,
                OrderDate = order.OrderDate,
                TransportMode = order.TransportMode
            };
            await LoadCatalogsAsync(viewModel,
                currentImporterId: order.ImporterId,
                currentCountryId: order.OriginCountryId,
                currentSupplierId: order.SupplierId,
                currentCurrencyId: order.CurrencyId);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImportationOrderEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadCatalogsAsync(viewModel, viewModel.ImporterId, viewModel.OriginCountryId, viewModel.SupplierId, viewModel.CurrencyId);
                return View(viewModel);
            }

            var dto = new ImportationOrderUpdateDTO
            {
                OrderId = viewModel.OrderId,
                ImporterId = viewModel.ImporterId,
                SupplierId = viewModel.SupplierId,
                OriginCountryId = viewModel.OriginCountryId,
                CurrencyId = viewModel.CurrencyId,
                OrderDate = viewModel.OrderDate,
                TransportMode = viewModel.TransportMode
            };

            var result = await _orderService.EditAsync(viewModel.OrderId, dto);

            TempData["Message"] = result.Message;
            TempData["TypeMessage"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadCatalogsAsync(viewModel, viewModel.ImporterId, viewModel.OriginCountryId, viewModel.SupplierId, viewModel.CurrencyId);
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderService.DeleteAsync(id);

            TempData["Message"] = result.Message;
            TempData["TypeMessage"] = result.TypeAlert;

            return RedirectToAction(nameof(Index));
        }
    }
}