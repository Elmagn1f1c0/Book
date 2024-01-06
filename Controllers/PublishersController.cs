using Books.Data.Services;
using Books.Data.Static;
using Books.Data.ViewModels;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

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



        //GET: publishers/edit/1
        public async Task<IActionResult> Edit(int id)
            {
                    var publisherDetails = await _service.GetByIdAsync(id);
                    if (publisherDetails == null) return View("NotFound");
                    return View(publisherDetails);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Publisher publisher, IFormFile bookPosterFile)
            {
                if (!ModelState.IsValid) return View(publisher);
            if (bookPosterFile != null && bookPosterFile.Length > 0)
            {
                if (!IsValidFileExtension(bookPosterFile))
                {
                    ModelState.AddModelError("BookPosterFile", "Invalid file type. Please upload an image file.");

                    return View(publisher);
                }

                publisher.ProfilePictureURL = await _imageService.UploadImageAsync(bookPosterFile, "images");
            }


            if (id == publisher.Id)
                {
                    await _service.UpdateAsync(id, publisher);
                    return RedirectToAction(nameof(Index));
                }
                return View(publisher);
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

        private bool IsValidFileExtension(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
    
}
