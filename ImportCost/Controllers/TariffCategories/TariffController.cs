using Microsoft.AspNetCore.Mvc;
using Application.DTOs.TarriffCategories;

using Application.Services.TarriffCategories;
using Application.ViewModel.TarriffCategories;
namespace ImportCost.Controllers.TariffCategories
{

    public class TariffController : Controller
    {
        private readonly TarriffCategoriesServices _tarriffCategories;

        public TariffController (TarriffCategoriesServices tarriffCategories)
        {
            _tarriffCategories = tarriffCategories;
        }

        public async Task<IActionResult> Index()
        {
            var listCategories = await _tarriffCategories.GetAllAsync();
            var listCategoriesViewModel = new List<ViewModelTarriffCategories>();
            foreach (var item in listCategories)
            {
                ViewModelTarriffCategories vt = new() { 
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
            if (t == null) return RedirectToRoute(new { controller = "Tariff", action = "Index" });

            ViewModelTarriffCategoriesSave tarriffCategoriesSave = new() { 
               TarriffCode = t.Key,
                Name = t.Name,
                State = t.State,
                TarriffPorcetage = t.PorcentageTariff,
                ITBIS = t.ITBIS,
                SelectiveTaxApplies = t.SelectiveTaxApplies,
                PorcentageTaxSelective = t.PorcentageTaxSelective ?? 0,
            };
            return View(tarriffCategoriesSave);
        }

        public async Task<IActionResult> Delete(string id) {

            var t = await _tarriffCategories.GetKeyAsync(id);
            if (t == null) return RedirectToRoute(new {controller = "Tariff", action ="Index"});
            return View(new ViewModelTarriffCategoriesDelete { TarriffCode = t.Key, Name = t.Name});
        
        }
        
        public async Task<IActionResult> Create()
        {
            return View("Save", new ViewModelTarriffCategories { 
                    key = "",
                    Name = "",
                    State = true,
                    ITBIS = false,
                    SelectiveTaxApplies = false,
                    PorcentageTariff = 0,
                    PorcentageTaxSelective = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModelTarriffCategoriesSave vt)
        {
            if (!ModelState.IsValid) return View("Save", vt);

            TariffCategoriesDto tDto = new()
            {
                Key = vt.TarriffCode,
                Name = vt.Name,
                State = vt.State,
                PorcentageTariff = vt.TarriffPorcetage,
                ITBIS = vt.ITBIS,
                SelectiveTaxApplies = vt.SelectiveTaxApplies,
                PorcentageTaxSelective = vt.PorcentageTaxSelective ?? 0
            };

            var result = await _tarriffCategories.EditAsync(tDto);
            if (!result.Success) return RedirectToRoute(new { controller = "Tariff", action = "Edit" });
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new { controller = "Tariff", action = "Index" });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(ViewModelTarriffCategoriesDelete vt)
        {

            if (!ModelState.IsValid) return View("Delete", vt);
            var tariff = await _tarriffCategories.DeleteAsync(vt.TarriffCode);
            if (!tariff.Success) return RedirectToRoute(new { controller = "Tariff", action = "Delete" });
            TempData["Message"] = tariff.Message;
            TempData["TypeAlert"] = tariff.TypeAlert;
            return RedirectToRoute(new {controller = "Tariff", action = "Index"});

        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewModelTarriffCategoriesSave vt)
        {
            if (!ModelState.IsValid) return View("Save", vt);

            TariffCategoriesDto tariffCategoriesDto = new () { 
                    Key = vt.TarriffCode.Trim(),
                    Name = vt.Name,
                    State = vt.State,
                    ITBIS = vt.ITBIS,
                    SelectiveTaxApplies = vt.SelectiveTaxApplies,
                    PorcentageTariff = vt.TarriffPorcetage,
                    PorcentageTaxSelective = vt.PorcentageTaxSelective ?? 0
            };

            var result = await _tarriffCategories.CreateAsync(tariffCategoriesDto);
            if (!result.Success) return RedirectToRoute(new {controller ="Tariff", action = "Save"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new { controller = "Tariff", action = "Index" });
        }
    }


}
