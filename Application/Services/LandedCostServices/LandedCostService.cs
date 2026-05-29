using Application.DTOs.LandedCost;
using Microsoft.Identity.Client;
using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;
using Persistence.Entities.ImportationOrderAndLandCost;
using Persistence.Repositories.Base;
using Persistence.Repositories.FinancialCore;
using Persistence.Repositories.ImportationOrderAndLandCost;
// Asegúrate de agregar los using de tus repositorios aquí

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

            //Obtener la orden (asumimos que el repositorio usa .Include() para traer detalles y gastos)
            var order = await _orderRepository.GetEntityById(orderId);
            if (order == null)
                throw new Exception("La orden no existe.");

            //Validar el estado de la orden
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("Solo se pueden calcular órdenes en estado Abierta.");

            //Validar que tenga productos
            if (order.ImportationOrderDetails == null || !order.ImportationOrderDetails.Any())
                throw new InvalidOperationException("La orden debe tener al menos un producto agregado.");

            //Validar que existan los gastos obligatorios (Flete y Seguro)
            if (order.ImportationExpenses == null || !order.ImportationExpenses.Any(e => e.ExpenseType == ExpenseType.FleteInternacional))
                throw new InvalidOperationException("La orden debe tener un gasto de tipo Flete internacional registrado.");

            if (!order.ImportationExpenses.Any(e => e.ExpenseType == ExpenseType.SeguroInternacional))
                throw new InvalidOperationException("La orden debe tener un gasto de tipo Seguro internacional registrado.");

        // Validar configuración de impuestos
        var taxConfig = await _taxConfigRepository.GetCurrentConfigAsync();
        if (taxConfig == null)
            throw new InvalidOperationException("Debe existir una configuración de impuestos activa.");

        // Identificar la moneda local
        var localCurrency = await _currencyRepository.GetLocalCurrencyAsync();
        if (localCurrency == null)
            throw new InvalidOperationException("No existe una moneda local configurada en el sistema.");
        //conversión de tasas de cambio.
        //Calcular FOB por Producto
        var ProductFOB = order.ImportationOrderDetails.ToDictionary(
                item => item.ProductId,
                item => item.Quantity * item.FOBUnitPrice
                );

            //Calcular FOB total
            decimal totalFob = ProductFOB.Values.Sum();

            //Validacion
            if (totalFob == 0)
            {
                throw new InvalidOperationException("El FOB total no puede ser cero.");
            }
            //Guardar tasa de cambio
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

        //FOB total en moneda local
        decimal TotalLocalFob = totalFob * ExchangeRateValue;
            //FOB individual en moneda local
            var LocalFobByProduct = ProductFOB.ToDictionary(
                item => item.Key,
                item => item.Value * ExchangeRateValue
            );

            //gastos a moneda local
            var LocalExpenses = new List<(ExpenseType Type, DistributionMethod Method, decimal LocalAmount)>();
            if (order.ImportationExpenses != null)
            {
                foreach (var expense in order.ImportationExpenses)
                {
                    decimal expenseRateValue;

                    if (expense.CurrencyId == localCurrency.Key)
                    {
                        expenseRateValue = 1m;
                    }
                else
                {
                    var expenseRate = await _exchangeRateRepository.GetLatestRateAsync(expense.CurrencyId, localCurrency.Key, expense.ImportationExpenseDate);
                    if (expenseRate == null)
                        throw new InvalidOperationException($"No se pudo obtener la tasa de cambio para el gasto {expense.ImportationExpenseId}.");

                    expenseRateValue = expenseRate.RateValue;
                }

                decimal localAmount = expense.ExpenseAmount * expenseRateValue;

                    LocalExpenses.Add((expense.ExpenseType, expense.DistributionMethod, localAmount));
                }
            }
            //CALCULO DE TOTALES

            //Peso total de la orden
            decimal TotalWeight = order.ImportationOrderDetails.Sum(item =>
                                  item.Quantity * (item.Product?.UnitWeight ?? 0m));
            //Volumen total de la orden
            decimal TotalVolume = order.ImportationOrderDetails.Sum(item =>
                                  item.Quantity * ((item.Product?.Large ?? 0m) * (item.Product?.Broad ?? 0m) * (item.Product?.High ?? 0m)));
            //Cantidad total de la orden
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

                    decimal assignedAmount = expense.LocalAmount * factor;

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

                decimal cif = localFob + alloc.AssignedFreight + alloc.AssignedInsurance;

            decimal porcentajeArancel = detail.Product?.tariffCategories?.PorcentageTariff ?? 0m;
            decimal arancel = cif * (porcentajeArancel / 100m);

            decimal impuestoSelectivo = 0m;
            bool aplicaSelectivo = detail.Product?.tariffCategories?.SelectiveTaxApplies ?? false;
            if (aplicaSelectivo)
            {
                decimal porcentajeSelectivo = detail.Product?.tariffCategories?.PorcentageTaxSelective ?? 0m;
                impuestoSelectivo = cif * (porcentajeSelectivo / 100m);
            }

            // Si el compañero le puso otro nombre a CustomsServiceFeePercentage en su entidad, ajustalo aquí
            decimal tasaServicioAduanal = cif * (taxConfig.CustomsServiceRatePercentage / 100m);

            decimal itbis = 0m;
            bool aplicaItbis = detail.Product?.tariffCategories?.ITBIS ?? false;
            if (aplicaItbis)
                {
                    decimal baseItbis = cif + arancel + impuestoSelectivo + tasaServicioAduanal;
                    itbis = baseItbis * (taxConfig.GeneralItbisPercentage / 100m);
                }

                decimal costoTotalImportado = localFob + alloc.AssignedFreight + alloc.AssignedInsurance +
                                  arancel + impuestoSelectivo + tasaServicioAduanal +
                                  itbis + alloc.AssignedLocalExpenses;

                decimal costoUnitarioImportado = detail.Quantity > 0 ? (costoTotalImportado / detail.Quantity) : 0m;

                decimal precioSugerido = costoUnitarioImportado;
                if (detail.ExpectedProfitMargin > 0 && detail.ExpectedProfitMargin < 100)
                {
                    precioSugerido = costoUnitarioImportado / (1m - (detail.ExpectedProfitMargin / 100m));
                }

                var detailDto = new LandedCostDetailDTO
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    OriginalTotalFob = originalFob,
                    LocalTotalFob = localFob,
                    TotalFreight = alloc.AssignedFreight,
                    TotalInsurance = alloc.AssignedInsurance,
                    TotalCif = cif,
                    TotalTariff = arancel,
                    TotalSelectiveTax = impuestoSelectivo,
                    TotalCustomsServiceFee = tasaServicioAduanal,
                    TotalItbis = itbis,
                    TotalLocalExpenses = alloc.AssignedLocalExpenses,
                    TotalImportationCost = costoTotalImportado,
                    TotalImportedQuantity = costoUnitarioImportado, // Asumiendo que esta propiedad la usas para el Costo Unitario
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
                // El LandedCostSummaryId y OrderId se asignarían al momento de guardar el cálculo final en BDD
                OrderId = order.OrderId,
                LocalCurrencyUsed = localCurrency.Key, // Asumiendo que la propiedad se llama IsoCode
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

        public async Task<bool> SaveOfficialCalculationAsync(string orderId, LandedCostSummaryDTO calculationResult)
        {


            // 1. Buscar la orden para validarla
            var order = await _orderRepository.GetEntityById(orderId);
            if (order == null)
                throw new Exception("La orden no existe.");

            // 2. Validar que la orden esté Abierta y no tenga ya un cálculo previo
            if (order.OrderState != OrderState.Abierta)
                throw new InvalidOperationException("Solo se pueden guardar cálculos de órdenes en estado Abierta.");

            // 3. Mapear el DTO a la Entidad real de Base de Datos
            // 1. Generamos el ID del cálculo maestro primero, para poder dárselo a los detalles
            string newSummaryId = Guid.NewGuid().ToString();

            // 3. Mapear el DTO a la Entidad real de Base de Datos
            var summaryEntity = new LandedCostSummary
            {
                LandedCostSummaryId = newSummaryId,
                OrderId = orderId,
                LocalCurrencyUsed = calculationResult.LocalCurrencyUsed,
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

                // Mapear la lista de detalles
                LandedCostDetails = calculationResult.ProductDetails.Select(d => new LandedCostDetail
                {
                    // CORRECCIÓN: Agregamos las dos propiedades 'required' que faltaban
                    LandedCostDetailId = Guid.NewGuid().ToString(),
                    LandedCostSummaryId = newSummaryId,

                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    OriginalFOB = d.OriginalTotalFob,
                    LocalFob = d.LocalTotalFob,
                    AssignedFreight = d.TotalFreight,
                    AssignedInsurance = d.TotalInsurance,
                    Cif = d.TotalCif,
                    Tariff = d.TotalTariff,
                    SelectiveTax = d.TotalSelectiveTax,
                    CustomsServiceFee = d.TotalCustomsServiceFee,
                    Itbis = d.TotalItbis,
                    AssignedLocalExpenses = d.TotalLocalExpenses,
                    TotalImportedCost = d.TotalImportationCost,
                    UnitImportedCost = d.TotalImportedQuantity,
                    DesiredMargin = d.DesiredMargin,
                    SuggestedSalePrice = d.SuggestedSalePrice
                }).ToList()
            };

            // 4. Cambiar el estado de la orden a Calculada
            order.OrderState = OrderState.Calculada;

            // 5. Guardar en Base de Datos (Opción A implementada)
            order.LandedCostSummary = summaryEntity;
            await _orderRepository.EditAsync(order);

            return true;
        }
    }