using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Exp.Models;
using AuthApp.Core.Exp.Repositories;

namespace AuthApp.Application.Exp.Interfaces
{
    public interface IBookService
    {
        // Task<UserModel> GetUserById(int userId);

        Task<BookModel> AddNewBook(IBookRepository bookRepo, CreateBookModel book);
        Task<BookModel> GetBookById(IBookRepository bookRepo, int bookId);
        Task<List<BookModel>> GetAllBooks(IBookRepository bookRepo);

        Task<LanguageModel> AddNewLanguage(IBookRepository bookRepo, CreateLanguageModel language);

        Task<BookGalleryModel> AddNewBookGallery(IBookRepository bookRepo, CreateBookGalleryModel bookGallery);

        /*Task<List<UserModel>> GetAllUsersForCustomer(int customerId);

        Task AddRole(int userId, int roleId);

        Task DeleteUser(int userId);*/
    }
}