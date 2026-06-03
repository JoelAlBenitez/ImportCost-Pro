using Application.DTOs.LandedCost;
using Application.Services.Result;
using Microsoft.Identity.Client;
using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Repositories.Base;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.ImportationOrderAndLandCost;


public class LandedCostService
{
    private readonly ImportationOrderRepository _orderRepository;
    private readonly CurrencyRepository _currencyRepository;
    private readonly ExchangeRateRepository _exchangeRateRepository;
    private readonly TaxConfigurationRepository _taxConfigRepository;

    public LandedCostService(
        ImportationOrderRepository orderRepository,
        CurrencyRepository currencyRepository,
        ExchangeRateRepository exchangeRateRepository,
        TaxConfigurationRepository taxConfigRepository)
    {
        _orderRepository = orderRepository;
        _currencyRepository = currencyRepository;
        _exchangeRateRepository = exchangeRateRepository;
        _taxConfigRepository = taxConfigRepository;
    }
    public class ExpenseAllocation
    {
        public decimal AssignedFreight { get; set; } = 0m;
        public decimal AssignedInsurance { get; set; } = 0m;
        public decimal AssignedLocalExpenses { get; set; } = 0m;
    }
    public async Task<LandedCostSummaryDTO> CalculateLandedCostAsync(string orderId)
    {
        var order = await _orderRepository.GetEntityById(orderId);
        if (order == null)
            throw new Exception("La orden no existe.");

        if (order.OrderState != OrderState.Abierta)
            throw new InvalidOperationException("Solo se pueden calcular órdenes en estado Abierta.");

        if (order.ImportationOrderDetails == null || !order.ImportationOrderDetails.Any())
            throw new InvalidOperationException("La orden debe tener al menos un producto agregado.");

        if (order.ImportationExpenses == null || !order.ImportationExpenses.Any(e => e.ExpenseType == ExpenseType.FleteInternacional))
            throw new InvalidOperationException("La orden debe tener un gasto de tipo Flete internacional registrado.");

        if (!order.ImportationExpenses.Any(e => e.ExpenseType == ExpenseType.SeguroInternacional))
            throw new InvalidOperationException("La orden debe tener un gasto de tipo Seguro internacional registrado.");

        if (order.ImportationExpenses.Any(e => e.DistributionMethod == DistributionMethod.PorVolumen))
        {
            if (order.ImportationOrderDetails.Any(d => (d.Product?.Large ?? 0m) <= 0 || (d.Product?.Broad ?? 0m) <= 0 || (d.Product?.High ?? 0m) <= 0))
                throw new InvalidOperationException("Existen gastos distribuidos por volumen, pero hay productos sin dimensiones (largo, ancho, alto) válidas configuradas.");
        }

        if (order.ImportationExpenses.Any(e => e.DistributionMethod == DistributionMethod.PorPeso))
        {
            if (order.ImportationOrderDetails.Any(d => (d.Product?.UnitWeight ?? 0m) <= 0))
                throw new InvalidOperationException("Existen gastos distribuidos por peso, pero hay productos sin peso unitario válido configurado.");
        }

        var taxConfig = await _taxConfigRepository.GetCurrentConfigAsync();
        if (taxConfig == null)
            throw new InvalidOperationException("Debe existir una configuración de impuestos activa.");

        var localCurrency = await _currencyRepository.GetLocalCurrencyAsync();
        if (localCurrency == null)
            throw new InvalidOperationException("No existe una moneda local configurada en el sistema.");

        var ProductFOB = order.ImportationOrderDetails.ToDictionary(
                item => item.ProductId,
                item => item.Quantity * item.FOBUnitPrice
                );

        decimal totalFob = ProductFOB.Values.Sum();

        if (totalFob == 0)
        {
            throw new InvalidOperationException("El FOB total no puede ser cero.");
        }
        decimal ExchangeRateValue;
        if (order.CurrencyId == localCurrency.Key)
        {
            ExchangeRateValue = 1m;
        }
        else
        {
            var exchangeRate = await _exchangeRateRepository.GetLatestRateAsync(order.CurrencyId, localCurrency.Key, order.OrderDate);
            if (exchangeRate == null)
                throw new InvalidOperationException("No se pudo obtener la tasa de cambio para la moneda de la orden.");

            ExchangeRateValue = exchangeRate.RateValue;
        }

        decimal TotalLocalFob = Math.Round(totalFob * ExchangeRateValue, 2, MidpointRounding.AwayFromZero);
        var LocalFobByProduct = ProductFOB.ToDictionary(
            item => item.Key,
            item => Math.Round(item.Value * ExchangeRateValue, 2, MidpointRounding.AwayFromZero)
        );

        var LocalExpenses = new List<(ExpenseType Type, DistributionMethod Method, decimal LocalAmount)>();
        if (order.ImportationExpenses != null)
        {
            var tasasNecesarias = order.ImportationExpenses
                .Where(e => e.CurrencyId != localCurrency.Key)
                .Select(e => new { e.CurrencyId, Fecha = e.ImportationExpenseDate.Date })
                .Distinct()
                .ToList();

            var diccionarioTasas = new Dictionary<string, decimal>();

            foreach (var req in tasasNecesarias)
            {
                var expenseRate = await _exchangeRateRepository.GetLatestRateAsync(req.CurrencyId, localCurrency.Key, req.Fecha);
                if (expenseRate == null)
                    throw new InvalidOperationException($"No se pudo obtener la tasa de cambio para la moneda ID {req.CurrencyId} en la fecha {req.Fecha:yyyy-MM-dd}.");

                diccionarioTasas[$"{req.CurrencyId}-{req.Fecha:yyyy-MM-dd}"] = expenseRate.RateValue;
            }

            foreach (var expense in order.ImportationExpenses)
            {
                decimal expenseRateValue;

                if (expense.CurrencyId == localCurrency.Key)
                {
                    expenseRateValue = 1m;
                }
                else
                {
                    string key = $"{expense.CurrencyId}-{expense.ImportationExpenseDate.Date:yyyy-MM-dd}";
                    expenseRateValue = diccionarioTasas[key];
                }

                decimal localAmount = Math.Round(expense.ExpenseAmount * expenseRateValue, 2, MidpointRounding.AwayFromZero);
                LocalExpenses.Add((expense.ExpenseType, expense.DistributionMethod, localAmount));
            }
        }

        decimal TotalWeight = order.ImportationOrderDetails.Sum(item =>
                                  item.Quantity * (item.Product?.UnitWeight ?? 0m));

        decimal TotalVolume = order.ImportationOrderDetails.Sum(item =>
                              item.Quantity * ((item.Product?.Large ?? 0m) * (item.Product?.Broad ?? 0m) * (item.Product?.High ?? 0m)));

        decimal TotalOrderQuantity = order.ImportationOrderDetails.Sum(item => item.Quantity);

        var allocations = new Dictionary<int, ExpenseAllocation>();
        foreach (var detail in order.ImportationOrderDetails)
        {
            if (!allocations.ContainsKey(detail.ProductId))
            {
                allocations[detail.ProductId] = new ExpenseAllocation();
            }
        }

        foreach (var expense in LocalExpenses)
        {
            foreach (var detail in order.ImportationOrderDetails)
            {
                decimal factor = 0m;
                switch (expense.Method)
                {
                    case DistributionMethod.PorValorFOB:
                        factor = TotalLocalFob > 0 ? (LocalFobByProduct[detail.ProductId] / TotalLocalFob) : 0m;
                        break;

                    case DistributionMethod.PorPeso:
                        decimal itemWeight = detail.Quantity * (detail.Product?.UnitWeight ?? 0m);
                        factor = TotalWeight > 0 ? (itemWeight / TotalWeight) : 0m;
                        break;

                    case DistributionMethod.PorVolumen:
                        decimal itemVolume = detail.Quantity * ((detail.Product?.Large ?? 0m) * (detail.Product?.Broad ?? 0m) * (detail.Product?.High ?? 0m));
                        factor = TotalVolume > 0 ? (itemVolume / TotalVolume) : 0m;
                        break;

                    case DistributionMethod.PorCantidad:
                        factor = TotalOrderQuantity > 0 ? (detail.Quantity / TotalOrderQuantity) : 0m;
                        break;
                }

                decimal assignedAmount = Math.Round(expense.LocalAmount * factor, 2, MidpointRounding.AwayFromZero);

                if (expense.Type == ExpenseType.FleteInternacional)
                {
                    allocations[detail.ProductId].AssignedFreight += assignedAmount;
                }
                else if (expense.Type == ExpenseType.SeguroInternacional)
                {
                    allocations[detail.ProductId].AssignedInsurance += assignedAmount;
                }
                else
                {
                    allocations[detail.ProductId].AssignedLocalExpenses += assignedAmount;
                }
            }
        }

        var productDetails = new List<LandedCostDetailDTO>();
        decimal totalCifGeneral = 0m, totalArancelGeneral = 0m, totalSelectivoGeneral = 0m;
        decimal totalServicioAduanalGeneral = 0m, totalItbisGeneral = 0m, totalImportationCostGeneral = 0m;
        decimal totalFreightGeneral = 0m, totalInsuranceGeneral = 0m, totalLocalExpensesGeneral = 0m;

        foreach (var detail in order.ImportationOrderDetails)
        {
            var alloc = allocations[detail.ProductId];
            decimal originalFob = detail.Quantity * detail.FOBUnitPrice;
            decimal localFob = LocalFobByProduct[detail.ProductId];

            decimal cif = Math.Round(localFob + alloc.AssignedFreight + alloc.AssignedInsurance, 2, MidpointRounding.AwayFromZero);

            decimal porcentajeArancel = detail.Product?.tariffCategories?.PorcentageTariff ?? 0m;
            decimal arancel = Math.Round(cif * (porcentajeArancel / 100m), 2, MidpointRounding.AwayFromZero);

            decimal impuestoSelectivo = 0m;
            bool aplicaSelectivo = detail.Product?.tariffCategories?.SelectiveTaxApplies ?? false;
            if (aplicaSelectivo)
            {
                decimal porcentajeSelectivo = detail.Product?.tariffCategories?.PorcentageTaxSelective ?? 0m;
                impuestoSelectivo = Math.Round(cif * (porcentajeSelectivo / 100m), 2, MidpointRounding.AwayFromZero);
            }

            decimal tasaServicioAduanal = Math.Round(cif * (taxConfig.CustomsServiceRatePercentage / 100m), 2, MidpointRounding.AwayFromZero);

            decimal itbis = 0m;
            bool aplicaItbis = detail.Product?.tariffCategories?.ITBIS ?? false;
            if (aplicaItbis)
            {
                decimal baseItbis = cif + arancel + impuestoSelectivo + tasaServicioAduanal;
                itbis = Math.Round(baseItbis * (taxConfig.GeneralItbisPercentage / 100m), 2, MidpointRounding.AwayFromZero);
            }

            decimal costoTotalImportado = Math.Round(localFob + alloc.AssignedFreight + alloc.AssignedInsurance +
                               arancel + impuestoSelectivo + tasaServicioAduanal +
                               itbis + alloc.AssignedLocalExpenses, 2, MidpointRounding.AwayFromZero);

            decimal costoUnitarioImportado = detail.Quantity > 0 ? Math.Round(costoTotalImportado / detail.Quantity, 2, MidpointRounding.AwayFromZero) : 0m;

            decimal precioSugerido = costoUnitarioImportado;
            if (detail.ExpectedProfitMargin > 0 && detail.ExpectedProfitMargin < 100)
            {
                precioSugerido = Math.Round(costoUnitarioImportado / (1m - (detail.ExpectedProfitMargin / 100m)), 2, MidpointRounding.AwayFromZero);
            }

            var detailDto = new LandedCostDetailDTO
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                OriginalTotalFob = originalFob,
                LocalTotalFob = localFob,
                AssignedFreight = alloc.AssignedFreight,
                AssignedInsurance = alloc.AssignedInsurance,
                TotalCif = cif,
                TotalTariff = arancel,
                TotalSelectiveTax = impuestoSelectivo,
                TotalCustomsServiceFee = tasaServicioAduanal,
                TotalItbis = itbis,
                AssignedLocalExpenses = alloc.AssignedLocalExpenses,
                TotalImportedCost = costoTotalImportado,
                UnitImportedCost = costoUnitarioImportado,
                DesiredMargin = detail.ExpectedProfitMargin,
                SuggestedSalePrice = precioSugerido
            };
            productDetails.Add(detailDto);

            totalCifGeneral += cif;
            totalArancelGeneral += arancel;
            totalSelectivoGeneral += impuestoSelectivo;
            totalServicioAduanalGeneral += tasaServicioAduanal;
            totalItbisGeneral += itbis;
            totalImportationCostGeneral += costoTotalImportado;
            totalFreightGeneral += alloc.AssignedFreight;
            totalInsuranceGeneral += alloc.AssignedInsurance;
            totalLocalExpensesGeneral += alloc.AssignedLocalExpenses;
        }

        var summaryDto = new LandedCostSummaryDTO
        {
            ImportationOrderId = order.OrderId,
            LocalCurrencyUsed = localCurrency.Key,
            ExchangeRate = ExchangeRateValue,
            OriginalTotalFob = totalFob,
            LocalTotalFob = TotalLocalFob,
            TotalFreight = totalFreightGeneral,
            TotalInsurance = totalInsuranceGeneral,
            TotalCif = totalCifGeneral,
            TotalTariff = totalArancelGeneral,
            TotalSelectiveTax = totalSelectivoGeneral,
            TotalCustomsServiceFee = totalServicioAduanalGeneral,
            TotalItbis = totalItbisGeneral,
            TotalLocalExpenses = totalLocalExpensesGeneral,
            TotalImportationCost = totalImportationCostGeneral,
            TotalImportedQuantity = TotalOrderQuantity,
            ProductDetails = productDetails
        };

        return summaryDto;
    }

    public async Task<ServiceResult> SaveOfficialCalculationAsync(string orderId, LandedCostSummaryDTO calculationResult)
    {
        var order = await _orderRepository.GetEntityById(orderId);

        if (order == null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = "La orden no existe.",
                TypeAlert = "error"
            };
        }
        if (order.OrderState != OrderState.Abierta)
        {
            return new ServiceResult
            {
                Success = false,
                Message = "Solo se pueden guardar cálculos de órdenes en estado Abierta.",
                TypeAlert = "warning"
            };
        }

        string newSummaryId = Guid.NewGuid().ToString();
        var summaryEntity = new LandedCostSummary
        {
            LandedCostSummaryId = newSummaryId,
            OrderId = orderId,
            LocalCurrencyId = calculationResult.LocalCurrencyUsed,
            ExchangeRate = calculationResult.ExchangeRate,
            OriginalTotalFob = calculationResult.OriginalTotalFob,
            LocalTotalFob = calculationResult.LocalTotalFob,
            TotalFreight = calculationResult.TotalFreight,
            TotalInsurance = calculationResult.TotalInsurance,
            TotalCif = calculationResult.TotalCif,
            TotalTariff = calculationResult.TotalTariff,
            TotalSelectiveTax = calculationResult.TotalSelectiveTax,
            TotalCustomsServiceFee = calculationResult.TotalCustomsServiceFee,
            TotalItbis = calculationResult.TotalItbis,
            TotalLocalExpenses = calculationResult.TotalLocalExpenses,
            TotalImportationCost = calculationResult.TotalImportationCost,
            TotalImportedQuantity = calculationResult.TotalImportedQuantity,

            LandedCostDetails = calculationResult.ProductDetails.Select(d => new LandedCostDetail
            {
                LandedCostDetailId = Guid.NewGuid().ToString(),
                LandedCostSummaryId = newSummaryId,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                OriginalFOB = d.OriginalTotalFob,
                LocalFob = d.LocalTotalFob,
                AssignedFreight = d.AssignedFreight,
                AssignedInsurance = d.AssignedInsurance,
                Cif = d.TotalCif,
                Tariff = d.TotalTariff,
                SelectiveTax = d.TotalSelectiveTax,
                CustomsServiceFee = d.TotalCustomsServiceFee,
                Itbis = d.TotalItbis,
                AssignedLocalExpenses = d.AssignedLocalExpenses,
                TotalImportedCost = d.TotalImportedCost,
                UnitImportedCost = d.UnitImportedCost,
                DesiredMargin = d.DesiredMargin,
                SuggestedSalePrice = d.SuggestedSalePrice
            }).ToList()
        };

        order.OrderState = OrderState.Calculada;

        order.LandedCostSummary = summaryEntity;
        await _orderRepository.EditAsync(order);

        return new ServiceResult
        {
            Success = true,
            Message = "El Landed Cost ha sido calculado y guardado exitosamente. La orden está ahora Calculada.",
            TypeAlert = "success"
        };
    }
}