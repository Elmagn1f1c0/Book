using Books.Models;

namespace Books.Data.ViewModels
{
    public class NewBookDropdownsVM
    {
        public NewBookDropdownsVM()
        {
            Publishers = new List<Publisher>();
            Stores = new List<Store>();
            Authors = new List<Author>();
        }

        public List<Publisher> Publishers { get; set; }
        public List<Store> Stores { get; set; }
        public List<Author> Authors { get; set; }
    }
}
