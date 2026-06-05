using Microsoft.AspNetCore.Mvc;
using Application.DTOs.TarriffCategories;

using Application.Services.TarriffCategories;
using Application.ViewModel.TarriffCategories;
namespace ImportCost.Controllers.TariffCategories
{

    public class TariffController : Controller
    {
        private readonly TarriffCategoriesServices _tarriffCategories;

        public TariffController(TarriffCategoriesServices tarriffCategories)
        {
            _tarriffCategories = tarriffCategories;
        }

        public async Task<IActionResult> Index()
        {
            var listCategories = await _tarriffCategories.GetAllAsync();
            var listCategoriesViewModel = new List<ViewModelTarriffCategories>();
            var dd = listCategories != null;
            foreach (var item in listCategories)
            {
                ViewModelTarriffCategories vt = new()
                {
                    key = item.Key,
                    Name = item.Name,
                    State = item.State,
                    PorcentageTariff = item.PorcentageTariff,
                    ITBIS = item.ITBIS,
                    SelectiveTaxApplies = item.SelectiveTaxApplies,
                    PorcentageTaxSelective = item.PorcentageTaxSelective ?? 0
                };
                listCategoriesViewModel.Add(vt);
            }
            return View(listCategoriesViewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var t = await _tarriffCategories.GetKeyAsync(id);
            if (t == null) return RedirectToAction(nameof(Index));

            ViewModelTarriffCategoriesSave tarriffCategoriesSave = new()
            {
                OldTariffCode = t.Key,
                TarriffCode = t.Key,
                Name = t.Name,
                State = t.State,
                TarriffPorcetage = t.PorcentageTariff,
                ITBIS = t.ITBIS,
                SelectiveTaxApplies = t.SelectiveTaxApplies,
                PorcentageTaxSelective = t.PorcentageTaxSelective ?? 0,
            };
            return View("Edit", tarriffCategoriesSave);
        }

        public async Task<IActionResult> Delete(string id)
        {

            var t = await _tarriffCategories.GetKeyAsync(id);
            if (t == null) return RedirectToAction(nameof(Index));
            return View("Delete", new ViewModelTarriffCategoriesDelete { TarriffCode = t.Key, Name = t.Name });

        }

        public async Task<IActionResult> Create()
        {
            return View("Save", new ViewModelTarriffCategoriesSave
            {
                TarriffCode = "",
                Name = "",
                State = true,
                ITBIS = false,
                SelectiveTaxApplies = false,
                TarriffPorcetage = 0,
                PorcentageTaxSelective = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelTarriffCategoriesSave vt)
        {
            if (!ModelState.IsValid)
            {

                return View("Edit", vt);
            }

            TariffCategoriesDto tDto = new()
            {
                Key = vt.TarriffCode,
                OldTariffCode = vt.OldTariffCode,
                Name = vt.Name,
                State = vt.State,
                PorcentageTariff = vt.TarriffPorcetage,
                ITBIS = vt.ITBIS,
                SelectiveTaxApplies = vt.SelectiveTaxApplies,
                PorcentageTaxSelective = vt.PorcentageTaxSelective ?? 0
            };

            var result = await _tarriffCategories.EditAsync(tDto);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Edit));
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelTarriffCategoriesDelete vt)
        {

            if (!ModelState.IsValid) return View("Delete", vt);
            var tariff = await _tarriffCategories.DeleteAsync(vt.TarriffCode);
            TempData["Message"] = tariff.Message;
            TempData["TypeAlert"] = tariff.TypeAlert;
            if (!tariff.Success) return RedirectToAction(nameof(Delete));

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelTarriffCategoriesSave vt)
        {
            if (!ModelState.IsValid) return View("Save", vt);

            TariffCategoriesDto tariffCategoriesDto = new()
            {
                Key = vt.TarriffCode.Trim(),
                Name = vt.Name,
                State = vt.State,
                ITBIS = vt.ITBIS,
                SelectiveTaxApplies = vt.SelectiveTaxApplies,
                PorcentageTariff = vt.TarriffPorcetage,
                PorcentageTaxSelective = vt.PorcentageTaxSelective ?? 0
            };

            var result = await _tarriffCategories.CreateAsync(tariffCategoriesDto);
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            if (!result.Success) return RedirectToAction(nameof(Create));

            return RedirectToAction(nameof(Index));
        }
    }


}