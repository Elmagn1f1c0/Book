using Books.Data.Services;
using Books.Data.Static;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Books.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
        public class PublishersController : Controller
        {
            private readonly IPublishersService _service;

            public PublishersController(IPublishersService service)
            {
                _service = service;
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
            public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Publisher publisher)
            {
                if (!ModelState.IsValid) return View(publisher);

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
            public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Publisher publisher)
            {
                if (!ModelState.IsValid) return View(publisher);

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
        }
    
}
