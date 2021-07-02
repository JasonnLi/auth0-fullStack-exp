using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.Core.Exp.Entities;
using AuthApp.Core.Exp.Repositories;
using AuthApp.Infra.Exp.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Infra.Exp.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly AuthAppContext _context;

        public BookRepository(AuthAppContext context)
        {
            this._context = context;
        }

        public async Task<Language> AddNewLanguage(Language language)
        {
            var newLanguage = new Language()
            {
                Name = language.Name,
                Description = language.Description,
            };

            var createdLanguage = await _context.Language.AddAsync(newLanguage);
            await _context.SaveChangesAsync();

            return createdLanguage.Entity;
        }

        //

        public async Task<Books> AddNewBook(Books book)
        {
            var newBook = new Books()
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                LanguageId = book.LanguageId,
                UserId = book.UserId,
                TotalPages = book.TotalPages
                
            };

            var createdBook = await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return createdBook.Entity;
        }

        public async Task<Books> GetBookById(int bookId)
        {
            return await this._context.Books.FirstOrDefaultAsync(
                b => b.Id == bookId);
        }

        public async Task<List<Books>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        //

        public async Task<BookGallery> AddNewBookGallery(BookGallery bookGallery)
        {
            var newBookGallery = new BookGallery()
            {
                BookId = bookGallery.BookId,
                Name = bookGallery.Name,
                URL = bookGallery.URL
            };

            var createdBookGallery = await _context.BookGallery.AddAsync(newBookGallery);
            await _context.SaveChangesAsync();

            return createdBookGallery.Entity;
        }
    }
}
