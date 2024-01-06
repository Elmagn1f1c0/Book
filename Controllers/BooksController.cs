using Books.Data;
using Books.Data.Services;
using Books.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{

    public class BooksController : Controller
    {
        private readonly IBooksService _service;
        public int PageSize = 4;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageService _imageService;
       // private readonly AppDbContext _dbContext;

        public BooksController(IBooksService service, IWebHostEnvironment hostEnvironment, ImageService imageService/*,AppDbContext dbContext*/)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
            _imageService = imageService;
           // _dbContext = dbContext;
        }

        
        public async Task<IActionResult> Index()
        {
            var allBooks = await _service.GetAllAsync(n => n.Store);
            return View(allBooks);
        }



        
        public async Task<IActionResult> Filter(string searchString)
        {
            var allBooks = await _service.GetAllAsync(n => n.Store);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allBooks.Where(n =>
                    n.Title.ToLower().Contains(searchString.ToLower()) ||
                    n.ISBN.ToLower().Contains(searchString.ToLower()) ||
                    n.YearPublished.ToString().Contains(searchString.ToLower()))
                    .ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allBooks);
        }

        //GET: Books/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var bookDetail = await _service.GetBookByIdAsync(id);
            return View(bookDetail);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                if (bookDropdownsData == null)
                {
                    ModelState.AddModelError("", "There was a problem getting the data for the dropdown lists.");
                    return View();
                }

                ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View();
           
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(NewBookVM bookVM, IFormFile bookPosterFile)
        {

            if (!ModelState.IsValid)
            {
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View(bookVM);
            }

            if (bookPosterFile != null && bookPosterFile.Length > 0)
            {
                if (!IsValidFileExtension(bookPosterFile))
                {
                    ModelState.AddModelError("BookPosterFile", "Invalid file type. Please upload an image file.");
                    var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                    ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                    ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                    ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                    return View(bookVM);
                }

                bookVM.ImageURL = await _imageService.UploadImageAsync(bookPosterFile, "images");
            }

            await _service.AddNewBookAsync(bookVM);
            return RedirectToAction(nameof(Index));

        }

        [Authorize]
        public IActionResult Publishers()
        {
            var publishers = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Publisher 1" },
        new SelectListItem { Value = "2", Text = "Publisher 2" },
        new SelectListItem { Value = "3", Text = "Publisher 3" }
    };

            ViewBag.Publishers = publishers;

            return View();
        }



        //GET: Books/Edit/1
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bookDetails = await _service.GetBookByIdAsync(id);
            if (bookDetails == null) return View("NotFound");

            var response = new NewBookVM()
            {
                Id = bookDetails.Id,
                Title = bookDetails.Title,
                ISBN = bookDetails.ISBN,
                Description = bookDetails.Description,
                Price = bookDetails.Price,
                YearPublished = bookDetails.YearPublished,
                EndDate = bookDetails.EndDate,
                ImageURL = bookDetails.ImageURL,
                BookCategory = bookDetails.BookCategory,
                StoreId = bookDetails.StoreId,
                PublisherId = bookDetails.PublisherId,
                AuthorIds = bookDetails.Authors_Books.Select(n => n.AuthorId).ToList(),
            };

            var bookDropdownsData = await _service.GetNewBookDropdownsValues();
            ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
            ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
            ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

            return View(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewBookVM bookVM, IFormFile bookPosterFile)
        {
            if (id != bookVM.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View(bookVM);
            }

            if (bookPosterFile != null && bookPosterFile.Length > 0)
            {
                if (!IsValidFileExtension(bookPosterFile)) 
                {
                    ModelState.AddModelError("BookPosterFile", "Invalid file type. Please upload an image file.");
                    var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                    ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                    ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                    ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                    return View(bookVM);
                }

                bookVM.ImageURL = await _imageService.UploadImageAsync(bookPosterFile, "images");
            }

            await _service.UpdateBookAsync(bookVM);
            return RedirectToAction(nameof(Index));
        }

        private bool IsValidFileExtension(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }


        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return View("NotFound");
            return View(book);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _service.GetByIdAsync(id);
                if (book == null)
                    return View("NotFound");

                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    // Handle the SQL exception
                    if (sqlException.Number == 547)
                    {
                        Console.WriteLine("Cannot delete the book because it is referenced in the ShoppingCartItems table.");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred while executing the DELETE statement: ");
                    }
                }
                else
                {
                    Console.WriteLine("An error occurred while updating the database: " + ex.Message);
                }
                return View("Error");
            }
        }


    }
}
