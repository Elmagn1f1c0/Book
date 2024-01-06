using Books.Data.Services;
using Books.Data.Static;
using Books.Data.ViewModels;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class PublishersController : Controller
    {
        private readonly IPublishersService _service;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageService _imageService;
        private readonly IConfiguration _configuration;

        public PublishersController(IPublishersService service, IWebHostEnvironment hostEnvironment, ImageService imageService, IConfiguration configuration)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
            _imageService = imageService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allPublishers = await _service.GetAllAsync();
            return View(allPublishers);
        }

        //GET: publishers/details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("NotFound");
            return View(publisherDetails);
        }

        //GET: publishers/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PublisherVM publisherVM)
        {
            if (!ModelState.IsValid)
                return View(publisherVM);

            if (publisherVM.BookPosterFile != null && publisherVM.BookPosterFile.Length > 0)
            {
                string folderPath = "publisher_images";
                var imageService = new ImageService(_configuration);
                publisherVM.ProfilePictureURL = await imageService.UploadImageAsync(publisherVM.BookPosterFile, folderPath);
            }

            Publisher publisher = new Publisher
            {
                FullName = publisherVM.FullName,
                Bio = publisherVM.Bio,
                ProfilePictureURL = publisherVM.ProfilePictureURL
            };

            await _service.AddAsync(publisher);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("NotFound");

            var publisherVM = new PublisherVM
            {
                Id = publisherDetails.Id,
                FullName = publisherDetails.FullName,
                Bio = publisherDetails.Bio,
                ProfilePictureURL = publisherDetails.ProfilePictureURL
            };

            return View(publisherVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PublisherVM publisherVM)
        {
            if (!ModelState.IsValid) return View(publisherVM);

            if (publisherVM.BookPosterFile != null && publisherVM.BookPosterFile.Length > 0)
            {
                string folderPath = "publisher_images";
                var imageService = new ImageService(_configuration);
                publisherVM.ProfilePictureURL = await imageService.UploadImageAsync(publisherVM.BookPosterFile, folderPath);
            }

            Publisher updatedPublisher = new Publisher
            {
                Id = publisherVM.Id,
                FullName = publisherVM.FullName,
                Bio = publisherVM.Bio,
                ProfilePictureURL = publisherVM.ProfilePictureURL
            };

            await _service.UpdateAsync(id, updatedPublisher);
            return RedirectToAction(nameof(Index));
        }




        //GET: publishers/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("NotFound");
            return View(publisherDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
