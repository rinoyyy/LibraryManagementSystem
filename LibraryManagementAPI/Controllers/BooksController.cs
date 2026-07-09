using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        private readonly ILogger<BooksController> _logger;

        public BooksController(
    IBookService bookService,
    ILogger<BooksController> logger)
        {
            _bookService = bookService;

            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            _logger.LogInformation("Fetching all books.");

            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("count")]
        public IActionResult GetBooksCount()
        {
            return Ok(_bookService.GetBookCount());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            Book? book = _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost] 
        public IActionResult AddBook(Book book)
        {
            _logger.LogInformation(
                "Adding new book: {Title}",
                book.Title);

            Book newBook = _bookService.AddBook(book);

            _logger.LogInformation(
                "Book added successfully with Id {Id}",
                newBook.Id);

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = newBook.Id },
                newBook);
        }

        [HttpGet("search")]
        public IActionResult SearchBooks(string keyword)
        {
            return Ok(_bookService.SearchBooks(keyword));
        }

        [HttpGet("sorted")]
        public IActionResult GetSortedBooks()
        {
            return Ok(_bookService.GetBooksSortedByYear());
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            var book = _bookService.UpdateBook(id, updatedBook);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var deleted = _bookService.DeleteBook(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        
        [Authorize(Roles = "Student")]
        [HttpPost("borrow")]
        public IActionResult BorrowBook(BorrowRequest request)
        {
            _logger.LogInformation(
                "Member {MemberId} is borrowing Book {BookId}",
                request.MemberId,
                request.BookId);

            var success =
                _bookService.BorrowBook(
                    request.BookId,
                    request.MemberId);

            if (!success)
            {
                _logger.LogWarning(
                    "Borrow failed for Member {MemberId} Book {BookId}",
                    request.MemberId,
                    request.BookId);

                return BadRequest("Borrow operation failed.");
            }

            _logger.LogInformation(
                "Borrow successful for Member {MemberId} Book {BookId}",
                request.MemberId,
                request.BookId);

            return Ok("Book borrowed successfully.");
        }

        [Authorize(Roles = "Student")]
        [HttpPost("return")]
        public IActionResult ReturnBook(BorrowRequest request)
        {
            var success = _bookService.ReturnBook(request.BookId, request.MemberId);

            if (!success)
            {
                return BadRequest("Return operation failed.");
            }

            return Ok("Book returned successfully.");
        }
    }
}