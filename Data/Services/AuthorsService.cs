using Books.Data.Base;
using Books.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;

namespace Books.Data.Services
{
    public class AuthorsService : EntityBaseRepository<Author>, IAuthorsService
    {
        public AuthorsService(AppDbContext context) : base(context) { }
    }
}
