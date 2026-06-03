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
                TempData["ErrorMessage"] = "Debes seleccionar una orden para calcular el costo.";
                return RedirectToAction("Index", "ImportationOrders");
            }

            try
            {
                var summary = await _landedCostService.CalculateLandedCostAsync(orderId);
                return View(summary);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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