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

        public OrderDetailsController(ImportationOrderDetailService detailService)
        {
            _importationOrderDetailService = detailService;
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
                OrderDetailId = detail.OrderDetailId, // Tu Primary Key del detalle
                OrderId = detail.OrderDetailId,
                ProductName = detail.ProductName,
                Quantity = detail.Quantity,
                FOBUnitPrice = detail.FOBUnitPrice,
                TotalFOB = detail.Quantity * detail.FOBUnitPrice // O si tu DTO ya lo trae calculado, pones ese
            }).ToList();

            ViewBag.CurrentOrderId = orderId;

            return View(viewModelList);
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
                OrderId = orderId // Precargamos el ID de la orden oculta en el formulario
            };

            await LoadProductsCatalogAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost] 
        public async Task<IActionResult> Create(OrderDetailCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadProductsCatalogAsync(viewModel);
                return View(viewModel);
            }
            var dto = new OrderDetailCreateDTO
            {
                ProductId = viewModel.ProductId,
                Quantity = viewModel.Quantity,
                FOBUnitPrice = viewModel.FOBUnitPrice,
                ExpectedProfitMargin = viewModel.ExpectedProfitMargin
            };

            var result = await _importationOrderDetailService.AddProductToOrderAsync(viewModel.OrderId, dto);

            if (!result.Success)
            {
                // Si falla (ej. el producto ya estaba en la orden, o la cantidad es 0)
                ModelState.AddModelError(string.Empty, result.Message);
                await LoadProductsCatalogAsync(viewModel);
                return View(viewModel);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });
        }


        private async Task LoadProductsCatalogAsync(OrderDetailCreateViewModel viewModel)
        {
            
            var products = await _productsService.GetAllAsync();
  
            viewModel.ProductsList = products.Select(p => new Application.ViewModel.Select.ViewModelSelectProducts
            {
                CodeReference = p.Key,
                ProductName = p.Name
            }).ToList();
        }

        // EDIT Muestra el formulario con los datos actuales
        [HttpGet]
        public async Task<IActionResult> Edit(string id) //es el OrderDetailId
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index", "ImportationOrders");

          
            var detail = await _importationOrderDetailService.GetDetailByIdAsync(id);

            if (detail == null)
            {
                TempData["ErrorMessage"] = "No se encontró el producto solicitado.";
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

            return View(viewModel);
        }

        // EDIT: Guarda las modificaciones
        [HttpPost] 
        public async Task<IActionResult> Edit(OrderDetailEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); 
            }

            var dto = new OrderDetailUpdateDTO
            {
                Quantity = viewModel.Quantity,
                FOBUnitPrice = viewModel.FOBUnitPrice,
                ExpectedProfitMargin = viewModel.ExpectedProfitMargin
            };

            try
            {
                
                await _importationOrderDetailService.EditProductInOrderAsync(viewModel.OrderDetailId, dto);

                TempData["SuccessMessage"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(viewModel);
            }
        }

        //DELETE Elimina un producto de la orden
        [HttpPost] 
        public async Task<IActionResult> Delete(string id, string orderId)
        {
            try
            {
                var result = await _importationOrderDetailService.RemoveProductFromOrderAsync(id);

                if (result)
                    TempData["SuccessMessage"] = "Producto eliminado de la orden.";
                else
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), new { orderId = orderId });
        }
    }
}

