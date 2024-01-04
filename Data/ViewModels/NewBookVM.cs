using Books.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Books.Data.ViewModels
{
    public class NewBookVM
    {
        public int Id { get; set; }

        [Display(Name = "Title name")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Display(Name = "Book description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; }

        [Display(Name = "Price in #")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        //[Display(Name = "Book poster URL")]
        //[Required(ErrorMessage = "Book poster URL is required")]
        public string ImageURL { get; set; }

        [Display(Name = "Book year published")]
        [Required(ErrorMessage = "Year Published is required")]
        public DateTime YearPublished { get; set; }

        [Display(Name = "Book end date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Book category is required")]
        public BookCategory BookCategory { get; set; }
        [Display(Name = "Book poster")]
        public IFormFile BookPosterFile { get; set; }

        //Relationships
        [Display(Name = "Select author(s)")]
        [Required(ErrorMessage = "Book author(s) is required")]
        public List<int> AuthorIds { get; set; }

        [Display(Name = "Select a store")]
        [Required(ErrorMessage = "Book store is required")]
        public int StoreId { get; set; }

        [Display(Name = "Select a publisher")]
        [Required(ErrorMessage = "Book publisher is required")]
        public int PublisherId { get; set; }
    }
}
