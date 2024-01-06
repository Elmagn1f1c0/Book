using Books.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Books.Data.Enums;

namespace Books.Models
{
    public class Book : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string ISBN { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public DateTime YearPublished { get; set; }
        public DateTime EndDate { get; set; }
        public BookCategory BookCategory { get; set; }


        //Relationships
        public List<Author_Book> Authors_Books { get; set; }

        //Store
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        //Publisher
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
       
    }
}

