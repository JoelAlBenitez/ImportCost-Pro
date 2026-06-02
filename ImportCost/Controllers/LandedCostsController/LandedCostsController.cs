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

        //Muestra el reporte matemático antes de guardarlo 
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

        //Guarda el cálculo oficial y cierra la orden
        [HttpPost] 
        public async Task<IActionResult> Save(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["ErrorMessage"] = "Debes seleccionar una orden válida.";
                return RedirectToAction("Index", "ImportationOrders");
            }

            try
            {
                var summary = await _landedCostService.CalculateLandedCostAsync(orderId);

                var success = await _landedCostService.SaveOfficialCalculationAsync(orderId, summary);

                if (success)
                {
                    TempData["SuccessMessage"] = "El Landed Cost ha sido calculado y la orden ha sido Cerrada exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar guardar el cálculo en la base de datos.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Después de calcular y cerrar, devolvemos al usuario al maestro de órdenes
            return RedirectToAction("Index", "ImportationOrders");
        }
    }
}