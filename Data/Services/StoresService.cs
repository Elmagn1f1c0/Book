using Books.Data.Base;
using Books.Models;
using System;

namespace Books.Data.Services
{
    public class StoresService : EntityBaseRepository<Store>, IStoresService
    {
        public StoresService(AppDbContext context) : base(context)
        {
        }
    }
}
