using Books.Data.Services;
using Books.Data.Static;
using Books.Data.ViewModels;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{

    [Authorize(Roles = UserRoles.Admin)]
        public class AuthorsController : Controller
        {
            private readonly IAuthorsService _service;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageService _imageService;
        private readonly IConfiguration _configuration;

        public AuthorsController(IAuthorsService service, IWebHostEnvironment hostEnvironment, ImageService imageService, IConfiguration configuration)
            {
                _service = service;
            _hostEnvironment = hostEnvironment;
            _imageService = imageService;
            _configuration = configuration;
        }

            [AllowAnonymous]
            public async Task<IActionResult> Index()
            {
                var data = await _service.GetAllAsync();
                return View(data);
            }

            //Get: Authors/Create
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(AuthorVM authorVM)
            {
                if (!ModelState.IsValid)
                {
                    return View(authorVM);
                }
            if (authorVM.BookPosterFile != null && authorVM.BookPosterFile.Length > 0)
            {
                string folderPath = "publisher_images";
                var imageService = new ImageService(_configuration);
                authorVM.ProfilePictureURL = await imageService.UploadImageAsync(authorVM.BookPosterFile, folderPath);
            }

            Author author = new Author
            {
                FullName = authorVM.FullName,
                Bio = authorVM.Bio,
                ProfilePictureURL = authorVM.ProfilePictureURL
            };
            await _service.AddAsync(author);
                return RedirectToAction(nameof(Index));
            }

            //Get: Authors/Details/1
            [AllowAnonymous]
            public async Task<IActionResult> Details(int id)
            {
                var authorDetails = await _service.GetByIdAsync(id);

                if (authorDetails == null) return View("NotFound");
                return View(authorDetails);
            }

            //Get: Authors/Edit/1
            public async Task<IActionResult> Edit(int id)
            {
                var authorDetails = await _service.GetByIdAsync(id);
                if (authorDetails == null) return View("NotFound");
                return View(authorDetails);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Author author)
            {
                if (!ModelState.IsValid)
                {
                    return View(author);
                }
                await _service.UpdateAsync(id, author);
                return RedirectToAction(nameof(Index));
            }

            //Get: Authors/Delete/1
            public async Task<IActionResult> Delete(int id)
            {
                var authorDetails = await _service.GetByIdAsync(id);
                if (authorDetails == null) return View("NotFound");
                return View(authorDetails);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var authorDetails = await _service.GetByIdAsync(id);
                if (authorDetails == null) return View("NotFound");

                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
        }
    
}
