using Books.Data.Base;
using Books.Data.ViewModels;
using Books.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Books.Data.Services
{
    public class BooksService : EntityBaseRepository<Book>, IBooksService
    {
        private readonly AppDbContext _context;
        public BooksService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewBookAsync(NewBookVM data)
        {
            var newBook = new Book()
            {
                Title = data.Title,
                Description = data.Description,
                ISBN = data.ISBN,
                Price = data.Price,
                ImageURL = data.ImageURL,
                StoreId = data.StoreId,
                YearPublished = data.YearPublished,
                EndDate = data.EndDate,
                BookCategory = data.BookCategory,
                PublisherId = data.PublisherId
            };
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            //Add Book Authors
            foreach (var authorId in data.AuthorIds)
            {
                var newAuthorBook = new Author_Book()
                {
                    BookId = newBook.Id,
                    AuthorId = authorId
                };
                await _context.Authors_Books.AddAsync(newAuthorBook);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var bookDetails = await _context.Books
                .Include(c => c.Store)
                .Include(p => p.Publisher)
                .Include(am => am.Authors_Books).ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(n => n.Id == id);

            return bookDetails;
        }

        public async Task<NewBookDropdownsVM> GetNewBookDropdownsValues()
        {
            var response = new NewBookDropdownsVM()
            {
                Authors = await _context.Authors.OrderBy(n => n.FullName).ToListAsync(),
                Stores = await _context.Stores.OrderBy(n => n.Name).ToListAsync(),
                Publishers = await _context.Publishers.OrderBy(n => n.FullName).ToListAsync()
            };

            return response;
        }

        public async Task UpdateBookAsync(NewBookVM data)
        {
            var dbBook = await _context.Books.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbBook != null)
            {
                dbBook.Title = data.Title;
                dbBook.Description = data.Description;
                dbBook.ISBN = data.ISBN;
                dbBook.Price = data.Price;
                dbBook.ImageURL = data.ImageURL;
                dbBook.StoreId = data.StoreId;
                dbBook.YearPublished = data.YearPublished;
                dbBook.EndDate = data.EndDate;
                dbBook.BookCategory = data.BookCategory;
                dbBook.PublisherId = data.PublisherId;
                await _context.SaveChangesAsync();
            }

            //Remove existing authors
            var existingAuthorsDb = _context.Authors_Books.Where(n => n.BookId == data.Id).ToList();
            _context.Authors_Books.RemoveRange(existingAuthorsDb);
            await _context.SaveChangesAsync();

            //Add Book Authors
            foreach (var authorId in data.AuthorIds)
            {
                var newAuthorBook = new Author_Book()
                {
                    BookId = data.Id,
                    AuthorId = authorId
                };
                await _context.Authors_Books.AddAsync(newAuthorBook);
            }
            await _context.SaveChangesAsync();
        }
    }
}
