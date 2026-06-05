using Microsoft.AspNetCore.Mvc;
using Application.DTOs.LandedCost;
using System;
using System.Threading.Tasks;
using System.Linq;
using Application.Services.ImportationOrderServices;
using Persistence.Entities.Enums;

namespace ImportCost.Controllers
{
    public class LandedCostController : Controller
    {
        private readonly LandedCostService _landedCostService;
        private readonly ImportationOrderService _orderService;

        public LandedCostController(LandedCostService landedCostService, ImportationOrderService orderService)
        {
            _landedCostService = landedCostService;
            _orderService = orderService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();

            var openOrders = orders
                .Where(o => o.OrderState == OrderState.Abierta)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            ViewBag.OrdersList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(openOrders, "OrderId", "OrderId");

            return View();
        }





        [HttpGet]
        public async Task<IActionResult> Preview(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["Message"] = "Debes seleccionar una orden para calcular el costo.";
                TempData["TypeAlert"] = "warning";
                return RedirectToAction("Index", "LandedCost");
            }

            try
            {
                var summaryDTO = await _landedCostService.CalculateLandedCostAsync(orderId);

                var detailViewModels = summaryDTO.ProductDetails.Select(d => new ImportCost.ViewModels.LandedCosts.LandedCostDetailViewModel
                {

                    ProductName = "Prod. ID: " + d.ProductId.ToString(),

                    Quantity = d.Quantity,
                    OriginalTotalFob = d.OriginalTotalFob,
                    LocalTotalFob = d.LocalTotalFob,
                    AssignedFreight = d.AssignedFreight,
                    AssignedInsurance = d.AssignedInsurance,
                    TotalCif = d.TotalCif,
                    TotalTariff = d.TotalTariff,
                    TotalSelectiveTax = d.TotalSelectiveTax,
                    TotalCustomsServiceFee = d.TotalCustomsServiceFee,
                    TotalItbis = d.TotalItbis,
                    AssignedLocalExpenses = d.AssignedLocalExpenses,
                    TotalImportedCost = d.TotalImportedCost,
                    UnitImportedCost = d.UnitImportedCost,
                    DesiredMargin = d.DesiredMargin,
                    SuggestedSalePrice = d.SuggestedSalePrice
                }).ToList();

                var viewModel = new ImportCost.ViewModels.LandedCosts.LandedCostSummaryViewModel
                {
                    OrderId = summaryDTO.ImportationOrderId,
                    LocalCurrencyUsed = summaryDTO.LocalCurrencyUsed.ToString(),
                    ExchangeRate = summaryDTO.ExchangeRate,
                    OriginalTotalFob = summaryDTO.OriginalTotalFob,
                    LocalTotalFob = summaryDTO.LocalTotalFob,
                    TotalFreight = summaryDTO.TotalFreight,
                    TotalInsurance = summaryDTO.TotalInsurance,
                    TotalCif = summaryDTO.TotalCif,
                    TotalTariff = summaryDTO.TotalTariff,
                    TotalSelectiveTax = summaryDTO.TotalSelectiveTax,
                    TotalCustomsServiceFee = summaryDTO.TotalCustomsServiceFee,
                    TotalItbis = summaryDTO.TotalItbis,
                    TotalLocalExpenses = summaryDTO.TotalLocalExpenses,
                    TotalImportationCost = summaryDTO.TotalImportationCost,
                    TotalImportedQuantity = summaryDTO.TotalImportedQuantity,
                    ProductDetails = detailViewModels 
                };

                return View(viewModel);
            }
        
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Index", "ImportationOrders");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["Message"] = "Debes seleccionar una orden válida.";
                TempData["TypeAlert"] = "warning";
                return RedirectToAction("Index", "ImportationOrders");
            }

            try
            {
                var summary = await _landedCostService.CalculateLandedCostAsync(orderId);

                var result = await _landedCostService.SaveOfficialCalculationAsync(orderId, summary);

                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error al procesar el costo: " + ex.Message;
                TempData["TypeAlert"] = "error";
            }

            return RedirectToAction("Index", "ImportationOrders");
        }
    }
}