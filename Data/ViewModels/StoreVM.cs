using Books.Models;
using System.ComponentModel.DataAnnotations;

namespace Books.Data.ViewModels
{
    public class StoreVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Book Poster is required")]
        public IFormFile BookPosterFile { get; set; }
        public string Logo { get; set; }

        [Display(Name = "Store Name")]
        [Required(ErrorMessage = "Store name is required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Store description is required")]
        public string Description { get; set; }

        //Relationships
        public List<Book> Books { get; set; }
    }
}
