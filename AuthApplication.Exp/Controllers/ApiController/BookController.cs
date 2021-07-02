using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Controllers;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Models;
using AuthApp.Core.Exp.Entities;
using AuthApp.Infra.Exp.Data;
using AuthApp.Infra.Exp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Application.Exp.Controllers.ApiController
{
    [ApiController]
    [Route("/books")]
    public class BookController : ApiControllerBase
    {
        private readonly AuthAppContext context;
        private readonly IBookService bookService;

        public BookController(
            AuthAppContext context,
            IBookService bookService)
        {
            this.context = context;
            this.bookService = bookService;
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> PostBook([FromBody] CreateBookModel body)
        {
            var bookRepo = new BookRepository(context);
            var book = await bookService.AddNewBook(bookRepo, new CreateBookModel()
            {
                Title = body.Title,
                Author = body.Author,
                Description = body.Description,
                TotalPages = body.TotalPages,
                LanguageId = body.LanguageId,
                UserId = body.UserId
            });

            return Ok(book);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetBooks()
        {
            var bookRepo = new BookRepository(context);
            var books = await bookService.GetAllBooks(bookRepo);

            return Ok(books);
        }

        [Authorize]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var bookRepo = new BookRepository(context);
            var book = await bookService.GetBookById(bookRepo, bookId);

            return Ok(book);
        }

        [Authorize]
        [HttpPost("addLanguage")]
        public async Task<IActionResult> PostLanguage([FromBody] CreateLanguageModel body)
        {
            var bookRepo = new BookRepository(context);
            var language = await bookService.AddNewLanguage(bookRepo, new CreateLanguageModel()
            {
                Name = body.Name,
                Description = body.Description
            });

            return Ok(language);
        }

        [Authorize]
        [HttpPost("addBookGallery")]
        public async Task<IActionResult> PostBookGallery([FromBody] CreateBookGalleryModel body)
        {
            var bookRepo = new BookRepository(context);
            var bookGallery = await bookService.AddNewBookGallery(bookRepo, new CreateBookGalleryModel()
            {
                BookId = body.BookId,
                Name = body.Name,
                URL = body.URL
            });

            return Ok(bookGallery);
        }

    }
}
