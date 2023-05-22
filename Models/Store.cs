using Books.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Books.Models
{
    public class Store : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Store Logo")]
        [Required(ErrorMessage = "Store logo is required")]
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
