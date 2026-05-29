using Application.DTOs.LandedCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
    {
        public interface ILandedCostService
        {
            // Ejecuta la matemática y devuelve el DTO para mostrarlo en pantalla
            Task<LandedCostSummaryDTO> CalculateLandedCostAsync(string orderId);

            // Toma el cálculo aprobado y lo guarda permanentemente (cambiando el estado a Calculada)
            Task<bool> SaveOfficialCalculationAsync(string orderId, LandedCostSummaryDTO calculationResult);
        }
    }

