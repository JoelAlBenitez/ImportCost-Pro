    using Application.DTOs.Expenses;
using Application.Services.Currencies;
using Application.Services.ImportationExpenseServices;
using Application.Services.ImportationOrderServices;
using ImportCost.ViewModels.ImportationExpenses;
using Microsoft.AspNetCore.Mvc;

namespace ImportCost.Controllers
{
    public class ImportationExpensesController : Controller
    {
        private readonly ImportationExpenseService _expenseService;
        private readonly CurrencyService _currenciesService;
        private readonly ImportationOrderService _orderService; 

        public ImportationExpensesController(
            ImportationExpenseService expenseService,
            CurrencyService currenciesService,
            ImportationOrderService orderService)
        {
            _expenseService = expenseService;
            _currenciesService = currenciesService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                TempData["Message"] = "Debes seleccionar una orden válida para ver sus gastos.";
                TempData["TypeAlert"] = "danger";

                return RedirectToAction("Index", "ImportationOrders");
            }

            var expensesList = await _expenseService.GetExpensesByOrderIdAsync(orderId);

            var viewModelList = expensesList.Select(e => new ExpenseViewModel
            {
                ImportationExpenseId = e.ImportationExpenseId,
                OrderId = e.OrderId,
                ExpenseType = e.ExpenseType.ToString(),
                ExpenseAmount = e.ExpenseAmount,
                CurrencyCode = e.CurrencyCode ?? "N/A",
                DistributionMethod = e.DistributionMethod.ToString(),
                ExpenseDate = e.ExpenseDate
            }).ToList();

            ViewBag.CurrentOrderId = orderId;
            return View("Index", viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return RedirectToAction("Index", "ImportationOrders");

            var viewModel = new ExpenseCreateViewModel
            {
                OrderId = orderId,
                ExpenseDate = DateTime.Today
            };

            await LoadCatalogsAsync(viewModel);
            return View("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCreateViewModel viewModel)
        {

            if (!await IsOrderEditable(viewModel.OrderId))
            {
                TempData["Message"] = "Acción prohibida: Esta orden no permite modificaciones.";
                TempData["TypeAlert"] = "danger";
                return RedirectToAction("Index", "ImportationOrders");
            }

            if (!ModelState.IsValid)
            {
                await LoadCatalogsAsync(viewModel);
                return View("Create", viewModel);
            }

            var dto = new ImportationExpenseCreateDTO
            {
                OrderId = viewModel.OrderId,
                ExpenseType = viewModel.ExpenseType,
                ExpenseAmount = viewModel.ExpenseAmount,
                CurrencyId = viewModel.CurrencyId,
                DistributionMethod = viewModel.DistributionMethod,
                ExpenseDate = viewModel.ExpenseDate
            };

            var result = await _expenseService.AddExpenseToOrderAsync(dto);

            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;

            if (!result.Success)
            {
                await LoadCatalogsAsync(viewModel);
                return View("Create", viewModel);
            }

            return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index", "ImportationOrders");

            var expense = await _expenseService.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                TempData["ErrorMessage"] = "No se encontró el gasto solicitado.";
                return RedirectToAction("Index", "ImportationOrders");
            }

            var viewModel = new ExpenseEditViewModel
            {
                ImportationExpenseId = expense.ImportationExpenseId,
                OrderId = expense.OrderId,
                ExpenseType = expense.ExpenseType,
                ExpenseAmount = expense.ExpenseAmount,
                CurrencyId = expense.CurrencyId,
                DistributionMethod = expense.DistributionMethod,
                ExpenseDate = expense.ExpenseDate
            };

            await LoadCatalogsAsync(viewModel);
            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExpenseEditViewModel viewModel)
        {

            if (!await IsOrderEditable(viewModel.OrderId))
            {
                TempData["Message"] = "Acción prohibida: Esta orden no permite modificaciones.";
                TempData["TypeAlert"] = "danger";
                return RedirectToAction("Index", "ImportationOrders");
            }

            if (!ModelState.IsValid)
            {
                await LoadCatalogsAsync(viewModel, viewModel.CurrencyId);
                return View("Edit", viewModel);
            }

            var dto = new ImportationExpenseUpdateDTO
            {
                ImportationExpenseId = viewModel.ImportationExpenseId,
                ExpenseType = viewModel.ExpenseType,
                ExpenseAmount = viewModel.ExpenseAmount,
                CurrencyId = viewModel.CurrencyId,
                DistributionMethod = viewModel.DistributionMethod,
                ExpenseDate = viewModel.ExpenseDate
            };

            try
            {
                var result = await _expenseService.EditExpenseAsync(dto);

                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;

                if (!result.Success)
                {
                    await LoadCatalogsAsync(viewModel);
                    return View("Edit", viewModel);
                }

                return RedirectToAction(nameof(Index), new { orderId = viewModel.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error inesperado: " + ex.Message;
                TempData["TypeAlert"] = "error";
                await LoadCatalogsAsync(viewModel);
                return View("Edit", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string orderId)
        {
            if (!await IsOrderEditable(orderId))
            {
                TempData["Message"] = "Acción prohibida: Esta orden no permite modificaciones.";
                TempData["TypeAlert"] = "danger";
                return RedirectToAction("Index", "ImportationOrders");
            }

            try
            {
                var result = await _expenseService.RemoveExpenseAsync(id);

                TempData["Message"] = result.Message;
                TempData["TypeAlert"] = result.TypeAlert;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error inesperado en el servidor: " + ex.Message;
                TempData["TypeAlert"] = "error";
            }

            return RedirectToAction(nameof(Index), new { orderId = orderId });
        }

        private async Task LoadCatalogsAsync(dynamic viewModel, int? currentCurrencyId = null)
        {
            var currencies = await _currenciesService.GetAllAsync();
            viewModel.CurrenciesList = currencies
                .Where(c => c.State == true || c.Key == currentCurrencyId)
                .Select(c => new Application.ViewModel.Select.ViewModelSelectCurrency
                {
                    Id = c.Key,
                    NameCurrency = c.Name
                }).ToList();
        }

        private async Task<bool> IsOrderEditable(string orderId)
        {
            var order = await _orderService.GetEntityById(orderId);
            return order != null && order.OrderState == Persistence.Entities.Enums.OrderState.Abierta;
        }
    }
}