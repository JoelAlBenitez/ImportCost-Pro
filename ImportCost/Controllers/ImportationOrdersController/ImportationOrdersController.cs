using Application.DTOs.Orders;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ImportationOrderServices;
using Application.Services.Importers;
using Application.Services.SuppliersServices;
using ImportCost.ViewModels.ImportationOrders;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities.Enums;
using Persistence.Entities.OperationalCommercial;

namespace ImportCost.Controllers.ImportationOrdersController
{
    public class ImportationOrdersController : Controller
    {
        private readonly ImportationOrderService _orderService;

        public ImportationOrdersController(
            ImportationOrderService orderService)
        {
            _orderService = orderService;
        }

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

            return View("Index", viewModelList);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ImportationOrderCreateViewModel
            {
                OrderId = string.Empty,
                OrderDate = DateTime.Today
            };
            await LoadCatalogsAsync(viewModel);
            return View(viewModel);
        }


        [HttpPost]
            public async Task<IActionResult> Create(ImportationOrderCreateViewModel viewModel)
            {
                
                if (!ModelState.IsValid)
                {
                    await LoadCatalogsAsync(viewModel);
                return View("Create", viewModel);
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
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadCatalogsAsync(viewModel);
                return View("Create", viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCatalogsAsync(ImportationOrderCreateViewModel viewModel)
        {
            viewModel.ImportersList = await _orderService.GetImportersForSelectAsync();
            viewModel.CountriesList = await _orderService.GetCountriesForSelectAsync();
            viewModel.SuppliersList = await _orderService.GetSuppliersForSelectAsync();
            viewModel.CurrenciesList = await _orderService.GetCurrenciesForSelectAsync();
        }
        private async Task LoadCatalogsAsync(ImportationOrderEditViewModel viewModel,
            int? currentImporterId = null,
            int? currentCountryId = null,
            int? currentSupplierId = null,
            int? currentCurrencyId = null)
        {
            viewModel.ImportersList = await _orderService.GetImportersForSelectAsync(currentImporterId);
            viewModel.CountriesList = await _orderService.GetCountriesForSelectAsync(currentCountryId);
            viewModel.SuppliersList = await _orderService.GetSuppliersForSelectAsync(currentSupplierId);
            viewModel.CurrenciesList = await _orderService.GetCurrenciesForSelectAsync(currentCurrencyId);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var validationResult = await _orderService.ValidateOrderForEditAsync(id);

            if (!validationResult.Success)
            {
                TempData["Message"] = validationResult.Message;
                TempData["TypeAlert"] = validationResult.TypeAlert;
                return RedirectToAction(nameof(Index));
            }

            var order = await _orderService.GetEntityById(id);

            var viewModel = new ImportationOrderEditViewModel
            {
                OriginalOrderId = order.OrderId,
                OrderId = order.OrderId,
                ImporterId = order.ImporterId,
                SupplierId = order.SupplierId,
                OriginCountryId = order.OriginCountryId,
                CurrencyId = order.CurrencyId,
                OrderDate = order.OrderDate,
                TransportMode = order.TransportMode,
                OrderState = order.OrderState
            };

            await LoadCatalogsAsync(viewModel,
                currentImporterId: order.ImporterId,
                currentCountryId: order.OriginCountryId,
                currentSupplierId: order.SupplierId,
                currentCurrencyId: order.CurrencyId);

            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImportationOrderEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadCatalogsAsync(viewModel, viewModel.ImporterId, viewModel.OriginCountryId, viewModel.SupplierId, viewModel.CurrencyId);
                return View("Edit", viewModel);
            }

            var dto = new ImportationOrderUpdateDTO
            {
                OrderId = viewModel.OrderId!,
                ImporterId = viewModel.ImporterId!.Value,
                SupplierId = viewModel.SupplierId!.Value,
                OriginCountryId = viewModel.OriginCountryId!.Value,
                CurrencyId = viewModel.CurrencyId!.Value,
                OrderDate = viewModel.OrderDate,
                TransportMode = viewModel.TransportMode!.Value
            };

            var result = await _orderService.EditAsync(viewModel.OrderId!, dto);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadCatalogsAsync(viewModel, viewModel.ImporterId, viewModel.OriginCountryId, viewModel.SupplierId, viewModel.CurrencyId);
                return View("Edit", viewModel);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderService.DeleteAsync(id);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CloseOrder(string orderId)
        {
            try
            {
                var result = await _orderService.CloseOrderAsync(orderId);

                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error inesperado: " + ex.Message;
                TempData["TypeAlert"] = "danger";
                return RedirectToAction("Index");
            }
        }
    }
}