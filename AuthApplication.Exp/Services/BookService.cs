using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Models;
using AuthApp.Core.Exp.Entities;
using AuthApp.Core.Exp.Repositories;

namespace AuthApp.Application.Exp.Services
{
    public class BookService : IBookService
    {

        public async Task<BookModel> AddNewBook(
            IBookRepository bookRepository,
            CreateBookModel createBookModel)
        {

            // createUser() return the whole entity to user
            var book = await bookRepository.AddNewBook(new Books()
            {
                Title = createBookModel.Title,
                Author = createBookModel.Author,
                Description = createBookModel.Description,
                TotalPages = createBookModel.TotalPages,
                LanguageId = createBookModel.LanguageId,
                UserId = createBookModel.UserId
            });

            return book.ToBookModel();
        }

        public async Task<BookModel> GetBookById(IBookRepository bookRepository, int  bookId)
        {
            var book = await bookRepository.GetBookById(bookId);
            return book.ToBookModel();
        }

        public async Task<List<BookModel>> GetAllBooks(IBookRepository bookRepository)
        {
            var books = await bookRepository.GetAllBooks();
            return books.ConvertAll(book => book.ToBookModel());
        }

        public async Task<LanguageModel> AddNewLanguage(
            IBookRepository bookRepository,
            CreateLanguageModel createLanguageModel)
        {

            // createUser() return the whole entity to user
            var language = await bookRepository.AddNewLanguage(new Language()
            {
                Name = createLanguageModel.Name,
                Description = createLanguageModel.Description
            });

            return language.ToLanguageModel();
        }

        public async Task<BookGalleryModel> AddNewBookGallery(
            IBookRepository bookRepository,
            CreateBookGalleryModel createBookGalleryModel)
        {

            // createUser() return the whole entity to user
            var bookGallery = await bookRepository.AddNewBookGallery(new BookGallery()
            {
                BookId = createBookGalleryModel.BookId,
                Name = createBookGalleryModel.Name,
                URL = createBookGalleryModel.URL
            });

            return bookGallery.ToBookGalleryModel();
        }

    }
}