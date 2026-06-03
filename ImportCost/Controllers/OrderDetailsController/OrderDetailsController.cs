using Application.DTOs.OrderDetails;
using Application.Services.ImportationOrderDetailServices;
using Application.Services.Importers;
using Application.Services.ProductsServices;
using ImportCost.ViewModels.OrderDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;


namespace ImportCost.Controllers.OrderDetailsController
{
    public class OrderDetailsController : Controller
    {
        // DEPENDENCIAS
        private readonly ImportationOrderDetailService _importationOrderDetailService;
        private readonly ProductsServices _productsService;

        public OrderDetailsController(ImportationOrderDetailService detailService, ProductsServices productsService)
        {
            _importationOrderDetailService = detailService;
            _productsService = productsService;
        }

        //Index

        [HttpGet]
        public async Task<IActionResult> Index(string orderId) 
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["ErrorMessage"] = "Debes seleccionar una orden para ver sus detalles.";
                return RedirectToAction("Index", "ImportationOrders"); //vuelve al controlador maestro
            }

            var detailsList = await _importationOrderDetailService.GetDetailsByOrderIdAsync(orderId);

            var viewModelList = detailsList.Select(detail => new OrderDetailEditViewModel
            {
                OrderDetailId = detail.OrderDetailId,  
                OrderId = detail.OrderDetailId,
                ProductName = detail.ProductName,
                Quantity = detail.Quantity,
                FOBUnitPrice = detail.FOBUnitPrice,
                TotalFOB = detail.Quantity * detail.FOBUnitPrice  
            }).ToList();

            ViewBag.CurrentOrderId = orderId;

            return View("Index", viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> AddProductToOrderAsync(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return RedirectToAction("Index", "ImportationOrders");
            }

            var viewModel = new OrderDetailCreateViewModel
            {
                OrderId = orderId  
            };

            await LoadProductsCatalogAsync(viewModel);

            return View("AddProductToOrderAsync", viewModel);
        }

        [HttpPost] 
        public async Task<IActionResult> Create(OrderDetailCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadProductsCatalogAsync(viewModel);
                return View("Create", viewModel);
            }
            var dto = new OrderDetailCreateDTO
            {
                ProductId = viewModel.ProductId,
                Quantity = viewModel.Quantity,
                FOBUnitPrice = viewModel.FOBUnitPrice,
                ExpectedProfitMargin = viewModel.ExpectedProfitMargin
            };

            var result = await _importationOrderDetailService.AddProductToOrderAsync(viewModel.OrderId, dto);

            TempData["Message"] = result.Message;
            TempData["TypeMessage"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadProductsCatalogAsync(viewModel);
                return View("Create", viewModel);
            }
            return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });


        }


        private async Task LoadProductsCatalogAsync(OrderDetailCreateViewModel viewModel)
        {
            
            var products = await _productsService.GetAllAsync();
  
            viewModel.ProductsList = products
                .Where(p => p.State == true) // Solo productos activos
                .Select(p => new Application.ViewModel.Select.ViewModelSelectProducts
            {
                CodeReference = p.Key,
                ProductName = p.Name
            }).ToList();
        }

        // EDIT Muestra el formulario con los datos actuales
        [HttpGet]
        public async Task<IActionResult> Edit(string id)  
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index", "ImportationOrders");

          
            var detail = await _importationOrderDetailService.GetDetailByIdAsync(id);

            if (detail == null)
            {
                TempData["Message"] = "No se encontró el gasto solicitado.";
                TempData["TypeMessage"] = "danger"; 
                return RedirectToAction("Index", "ImportationOrders");
            }

            var viewModel = new OrderDetailEditViewModel
            {
                OrderDetailId = detail.OrderDetailId,
                OrderId = detail.OrderId,
                ProductName = detail.ProductName,
                Quantity = detail.Quantity,
                FOBUnitPrice = detail.FOBUnitPrice,
                ExpectedProfitMargin = detail.ExpectedProfitMargin
            };

            return View("Create", viewModel);
        }
 
        [HttpPost]
        public async Task<IActionResult> Edit(OrderDetailEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View("Edit", viewModel);

            var dto = new OrderDetailUpdateDTO
            {
                Quantity = viewModel.Quantity,
                FOBUnitPrice = viewModel.FOBUnitPrice,
                ExpectedProfitMargin = viewModel.ExpectedProfitMargin
            };

            try
            {
                var result = await _importationOrderDetailService.EditProductInOrderAsync(viewModel.OrderDetailId, dto);

                TempData["Message"] = result.Message;
                TempData["TypeMessage"] = result.TypeAlert;

                if (!result.Success) return View("Edit", viewModel);

                return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error inesperado: " + ex.Message;
                TempData["TypeMessage"] = "danger";
                return View("Edit", viewModel);
            }
        }

        //DELETE Elimina un producto de la orden
        [HttpPost] 
        public async Task<IActionResult> Delete(string id, string orderId)
        {
            try
            {
                var result = await _importationOrderDetailService.RemoveProductFromOrderAsync(id);

                TempData["Message"] = result.Message;

                TempData["TypeMessage"] = result.TypeAlert;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error inesperado: " + ex.Message;
                TempData["TypeMessage"] = "danger";
            }
            return RedirectToAction(nameof(Index), new { orderId = orderId });
        }
    }
}

