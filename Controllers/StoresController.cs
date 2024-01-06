using Books.Data.Services;
using Books.Data.Static;
using Books.Data.ViewModels;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageService _imageService;
        private readonly IConfiguration _configuration;

        public StoresController(IStoresService service, IWebHostEnvironment hostEnvironment, ImageService imageService, IConfiguration configuration)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
            _imageService = imageService;
            _configuration = configuration;
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
        public async Task<IActionResult> Create(StoreVM storeVM)
        {
            if (!ModelState.IsValid) return View(storeVM);
            if (storeVM.BookPosterFile != null && storeVM.BookPosterFile.Length > 0)
            {
                string folderPath = "publisher_images";
                var imageService = new ImageService(_configuration);
                storeVM.Logo = await imageService.UploadImageAsync(storeVM.BookPosterFile, folderPath);
            }

            Store store = new Store
            {
                Name = storeVM.Name,
                Description = storeVM.Description,
                Logo = storeVM.Logo
            };

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
            var storeVM = new StoreVM
            {
                Id = storeDetails.Id,
                Name = storeDetails.Name,
                Description = storeDetails.Description,
                Logo = storeDetails.Logo
            };

            return View(storeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, StoreVM storeVM)
        {
            if (!ModelState.IsValid) return View(storeVM);
            if (storeVM.BookPosterFile != null && storeVM.BookPosterFile.Length > 0)
            {
                string folderPath = "publisher_images";
                var imageService = new ImageService(_configuration);
                storeVM.Logo = await imageService.UploadImageAsync(storeVM.BookPosterFile, folderPath);
            }

            Store updatedStore = new Store
            {
                Id= storeVM.Id,
                Name = storeVM.Name,
                Description = storeVM.Description,
                Logo = storeVM.Logo
            };
            await _service.UpdateAsync(id, updatedStore);
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
