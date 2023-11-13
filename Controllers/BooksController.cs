using Books.Data.Services;
using Books.Data.Static;
using Books.Data.ViewModels;
using Books.Models;
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


        public BooksController(IBooksService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create(NewBookVM book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                    ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                    ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                    ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName"); 

                    return View(book);
                }

                await _service.AddNewBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("Input all fields");
            }
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
        [Authorize]
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
        public async Task<IActionResult> Edit(int id, NewBookVM book)
        {
            if (id != book.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                ViewBag.Stores = new SelectList(bookDropdownsData.Stores, "Id", "Name");
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "FullName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View(book);
            }

            await _service.UpdateBookAsync(book);
            return RedirectToAction(nameof(Index));
        }

        [Authorize/*(Roles = "Admin")*/]
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
                        Console.WriteLine("An error occurred while executing the DELETE statement: "/* + sqlException.Message*/);
                    }
                }
                else
                {
                    // Handle other DbUpdateException scenarios
                    Console.WriteLine("An error occurred while updating the database: " + ex.Message);
                }
                // Optionally, you can return an error view or redirect to an error page.
                return View("Error");
            }
        }

    }
}
