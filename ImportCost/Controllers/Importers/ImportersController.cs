using Microsoft.AspNetCore.Mvc;
using Application.Services.Importers;
using Application.ViewModel.Importers;
using Application.DTOs.Importers;
using Application.Services.Countries;
using Application.ViewModel.Select;
namespace ImportCost.Controllers.Importers
{
    public class ImportersController : Controller
    {

        private readonly ImportersServices _importersServices;
        private readonly CountryService _countriesServices;
        public ImportersController(ImportersServices importersServices, CountryService countryService)
        {
            _importersServices = importersServices;
            _countriesServices = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _importersServices.GetAllAsync();
            var listView = new List<ViewModelImporter>();
            foreach (var item in list)
            {
                ViewModelImporter viewModel = new() { 
                    key = item.Key,
                    Name = item.Name,
                    State = item.State,
                    Identification = item.Identification,
                    Phone = item.Phone,
                    Email = item.Email,
                    CountryName = item.CountryName!,
                    CountryId = item.CountryId
                };
                listView.Add(viewModel);
            }
            return View(listView);
        }

        private async Task<List<ViewModelSelectCountries>> GetCountries(int key = 0)
        {
            var list = new List<ViewModelSelectCountries>();
            var countries = await _countriesServices.GetAllAsync();
            foreach (var item in countries)
            {
               if(item.State || key != 0 && item.Key != key)
                {
                    ViewModelSelectCountries viewModelSelectCountries = new()
                    {
                        CountryId = item.Key,
                        CountryName = item.Name
                    };
                    list.Add(viewModelSelectCountries);
                }
            }
            return list;
        }

        public async Task<IActionResult> Create()
        {
            return View("Save", new ViewModelImporterSave
            {
                Name = "",
                Identifcation = "",
                State  = true,
                countryId = 0,
                Countries = await GetCountries(), 
                PhoneNumber = "",
                Email = "",
                Address = ""
            });

        }
        [HttpPost]
        public async Task<IActionResult> Create(ViewModelImporterSave vp)
        {
            if (!ModelState.IsValid) return RedirectToRoute(new {controller = "Importers", action = "Save"});
            ImporterDto importerDto = new (){ 
                Key = 0,
                Name = vp.Name,
                Identification = vp.Identifcation,
                CountryId = vp.countryId,
                Phone = vp.PhoneNumber,
                Email = vp.Email,
                Address = vp.Address,
                State = vp.State
            };
            var result = await _importersServices.CreateAsync(importerDto);
            if (!result.Success) return RedirectToRoute(new { controller = "Importers", action = "Save"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new {controller = "Importers", action = "Index"});
        }

        public async Task<IActionResult> Edit(int id) {
        
            var importes = await _importersServices.GetKeyAsync(id);
            if (importes == null) return RedirectToRoute(new { controller = "Importers", action = "Edit" });
            
            ViewModelImporterSave viewModelImporterSave = new() { 
                Key = importes.Key,
                Name = importes.Name,
                State = importes.State,
                Identifcation = importes.Identification,
                countryId = importes.CountryId,
                Address = importes.Address,
                PhoneNumber = importes.Phone,
                Email = importes.Email,
                Countries = await GetCountries(importes.CountryId)
            };
            return View("Edit", viewModelImporterSave);
        }
        [HttpPost]
        public async Task<IActionResult> Edit (ViewModelImporterSave viewModelImporterSave) {

            if (!ModelState.IsValid) return RedirectToRoute(new {controller = "Importers", action = "Edit"});
            ImporterDto importerDto = new () { 
                Key = viewModelImporterSave.Key,
                Name = viewModelImporterSave.Name,
                State = viewModelImporterSave.State,
                Identification = viewModelImporterSave.Identifcation,
                Phone = viewModelImporterSave.PhoneNumber,
                Address = viewModelImporterSave.Address,
                Email = viewModelImporterSave.Email,
                CountryId = viewModelImporterSave.countryId,
            };

            var result = await _importersServices.EditAsync(importerDto);
            if (!result.Success) return RedirectToRoute(new {controller = "Importers", action = "Edit"});
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return RedirectToRoute(new {controller = "Importers", action = "Index"});

        }
        [HttpPost]
        public async Task<IActionResult> Delete (ViewModelImportDelete vi)
        {
            if (!ModelState.IsValid) return View("Delete", vi);
            var result = await _importersServices.DeleteAsync(vi.Key);
            if (!result.Success) return RedirectToRoute(new { contorller = "Importers", action = "Delete" });
            TempData["Message"] = result.Message;
            TempData["TypeAlert"] = result.TypeAlert;
            return  RedirectToRoute(new {controller = "Importers", action = "Delete"});
        }
        public async Task<IActionResult> Delete( int key)
        {
            var import = await _importersServices.GetKeyAsync(key);
            if (import == null) return RedirectToRoute(new  { controller = "Impoters", action = "Index"});
            return View("Delete", new ViewModelImportDelete { Key = import.Key, Name = import.Name});
        }
    }
}
