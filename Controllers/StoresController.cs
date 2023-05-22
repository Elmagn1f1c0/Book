using Books.Data.Services;
using Books.Data.Static;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Books.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class StoresController : Controller
    {
        
       
            private readonly IStoresService _service;

            public StoresController(IStoresService service)
            {
                _service = service;
            }

            [AllowAnonymous]
            public async Task<IActionResult> Index()
            {
                var allStores = await _service.GetAllAsync();
                return View(allStores);
            }


            //Get: Stores/Create
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Store store)
            {
                if (!ModelState.IsValid) return View(store);
                await _service.AddAsync(store);
                return RedirectToAction(nameof(Index));
            }

            //Get: Stores/Details/1
            [AllowAnonymous]
            public async Task<IActionResult> Details(int id)
            {
                var storeDetails = await _service.GetByIdAsync(id);
                if (storeDetails == null) return View("NotFound");
                return View(storeDetails);
            }

            //Get: Stores/Edit/1
            public async Task<IActionResult> Edit(int id)
            {
                var storeDetails = await _service.GetByIdAsync(id);
                if (storeDetails == null) return View("NotFound");
                return View(storeDetails);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Store store)
            {
                if (!ModelState.IsValid) return View(store);
                await _service.UpdateAsync(id, store);
                return RedirectToAction(nameof(Index));
            }

            //Get: Stores/Delete/1
            public async Task<IActionResult> Delete(int id)
            {
                var storeDetails = await _service.GetByIdAsync(id);
                if (storeDetails == null) return View("NotFound");
                return View(storeDetails);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirm(int id)
            {
                var storeDetails = await _service.GetByIdAsync(id);
                if (storeDetails == null) return View("NotFound");

                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
    }
    
}
