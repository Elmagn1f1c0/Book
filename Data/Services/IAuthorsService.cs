using Books.Data.Base;
using Books.Models;

namespace Books.Data.Services
{
    public interface IAuthorsService : IEntityBaseRepository<Author>
    {
    }
}
