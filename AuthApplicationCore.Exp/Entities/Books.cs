using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Core.Exp.Entities
{
    public class Books
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        // The author 
        [Required]
        public string Author { get; set; }

        public string Description { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int TotalPages { get; set; }

        [Required]
        public DateTime CreateOn { get; set; }

        public Language Language { get; set; }
        public User User { get; set; }

        public ICollection<BookGallery> bookGallery { get; set; }

    }
}
