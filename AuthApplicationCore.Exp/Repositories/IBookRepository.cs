using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.Core.Exp.Entities;

namespace AuthApp.Core.Exp.Repositories
{
    public interface IBookRepository
    {
        public Task<Language> AddNewLanguage(Language language);

        public Task<Books> AddNewBook(Books book);
        public Task<Books> GetBookById(int bookId);
        public Task<List<Books>> GetAllBooks();

        public Task<BookGallery> AddNewBookGallery(BookGallery bookGallery);
    }
}
