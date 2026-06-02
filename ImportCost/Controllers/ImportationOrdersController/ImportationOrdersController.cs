using Application.DTOs.Orders;
using Application.Services.Countries;
using Application.Services.Currencies;
using Application.Services.ImportationOrderServices;
using Application.Services.Importers;
using Application.Services.SuppliersServices;
using ImportCost.ViewModels.ImportationOrders;
using Microsoft.AspNetCore.Mvc;

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

       
        //Cargar los catálogos 
        
        private async Task LoadCatalogsAsync(ImportationOrderCreateViewModel viewModel)
        {
  
            var importers = await _importersService.GetAllAsync();
            viewModel.ImportersList = importers.Select(i => new Application.ViewModel.Select.ViewModelSelectImporters
            {
                ImporterId = i.Key,
                ImporterName = i.Name
            }).ToList();

            var countries = await _countryService.GetAllAsync();
            viewModel.CountriesList = countries.Select(c => new Application.ViewModel.Select.ViewModelSelectCountries
            {
                CountryId = c.Key,
                CountryName = c.Name
            }).ToList();


            var suppliers = await _suppliersService.GetAllAsync();
            viewModel.SuppliersList = suppliers.Select(s => new Application.ViewModel.Select.ViewModelSelectSuppliers
            {
                SupplierId = s.Key,
                SupplierName = s.Name
            }).ToList();

            var currencies = await _currenciesService.GetAllAsync();
            viewModel.CurrenciesList = currencies.Select(c => new Application.ViewModel.Select.ViewModelSelectCurrency{
                Id = c.Key,
                NameCurrency = c.Name
            }).ToList();
        }
    }
}