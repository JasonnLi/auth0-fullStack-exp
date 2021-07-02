using AuthApp.Core.Exp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Models
{

    public class CreateBookModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int TotalPages { get; set; }
        public int LanguageId { get; set; }
        public int UserId { get; set; }
    }

    // by having CreateBookModel and BookModel, you are able to use different models in creating data and reading data
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public int UserId { get; set; }
        public int TotalPages { get; set; }
        public DateTime CreateOn { get; set; }
    }

    // used for converting the output mode, converting the output
    public static class BookModelMappings
    {
        public static BookModel ToBookModel(this Books book)
        {
            return new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                LanguageId = book.LanguageId,
                UserId = book.UserId,
                CreateOn = book.CreateOn
            };
        }
    }

    public class CreateLanguageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    
    public class LanguageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public static class LanguageModelMappings
    {
        public static LanguageModel ToLanguageModel(this Language language)
        {
            return new LanguageModel()
            {
                Id = language.Id,
                Name = language.Name,
                Description = language.Description
            };
        }
    }

    public class CreateBookGalleryModel
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public int BookId { get; set; }
    }

    public class BookGalleryModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
    }

    public static class BookGalleryModelMappings
    {
        public static BookGalleryModel ToBookGalleryModel(this BookGallery bookGallery)
        {
            return new BookGalleryModel()
            {
                Id = bookGallery.Id,
                BookId = bookGallery.BookId,
                Name = bookGallery.Name,
                URL = bookGallery.URL
            };
        }
    }
}
