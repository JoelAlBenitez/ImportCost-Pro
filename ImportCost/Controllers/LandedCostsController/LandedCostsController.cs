using Microsoft.AspNetCore.Mvc;
using Application.DTOs.LandedCost;
using System;
using System.Threading.Tasks;


namespace ImportCost.Controllers
{
    public class LandedCostController : Controller
    {
        private readonly LandedCostService _landedCostService;

        public LandedCostController(LandedCostService landedCostService)
        {
            _landedCostService = landedCostService;
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
                TempData["TypeMessage"] = "warning";
                return RedirectToAction("Index", "ImportationOrders");
            }

            try
            {
                var summary = await _landedCostService.CalculateLandedCostAsync(orderId);

                var result = await _landedCostService.SaveOfficialCalculationAsync(orderId, summary);

                TempData["Message"] = result.Message;
                TempData["TypeMessage"] = result.TypeAlert;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error al procesar el costo: " + ex.Message;
                TempData["TypeMessage"] = "error";
            }

            return RedirectToAction("Index", "ImportationOrders");
        }
    }
}