﻿using Books.Models;
using System.ComponentModel.DataAnnotations;

namespace Books.Data.ViewModels
{
    public class PublisherVM
    {
        [Key]
        public int Id { get; set; }
        public string ProfilePictureURL { get; set; }
        [Required(ErrorMessage = "Book Poster is required")]
        public IFormFile BookPosterFile { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 chars")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }

        //Relationships
        public List<Book> Books { get; set; }
    }
}
