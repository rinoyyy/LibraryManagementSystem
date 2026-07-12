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

        private readonly IUserService _userService;

        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookService bookService,
            IUserService userService,
            ILogger<BooksController> logger)
            {
                _bookService = bookService;
                _userService = userService;
                _logger = logger;
            }

        [HttpGet("count")]
        public IActionResult GetBooksCount()
        {
            return Ok(_bookService.GetBookCount());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            BookResponse? book = _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddBook(AddBookRequest request)
        {
            _logger.LogInformation(
                "Adding new book: {Title}",
                request.Title);

            BookResponse newBook = _bookService.AddBook(request);

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
        public IActionResult UpdateBook(int id, UpdateBookRequest request)
        {
            _logger.LogInformation(
                "Updating book with Id {Id}",
                id);

            BookResponse? book =
                _bookService.UpdateBook(id, request);

            if (book == null)
            {
                _logger.LogWarning(
                    "Book with Id {Id} not found",
                    id);

                return NotFound();
            }

            _logger.LogInformation(
                "Book with Id {Id} updated successfully",
                id);

            return Ok(book);
        }


        [Authorize(Roles = "Student")]
        [HttpPost("{id}/borrow")]
        public IActionResult BorrowBook(int id)
        {
            var username = User.Identity!.Name!;

            var memberId = _userService.GetMemberId(username);

            if (memberId == null)
            {
                _logger.LogWarning(
                    "Member not found for username {Username}",
                    username);

                return Unauthorized();
            }

            _logger.LogInformation(
                "Member {MemberId} is borrowing Book {BookId}",
                memberId,
                id);

            var success = _bookService.BorrowBook(id, memberId.Value);

            if (!success)
            {
                _logger.LogWarning(
                    "Borrow failed for Member {MemberId} Book {BookId}",
                    memberId,
                    id);

                return BadRequest("Borrow operation failed.");
            }

            _logger.LogInformation(
                "Borrow successful for Member {MemberId} Book {BookId}",
                memberId,
                id);

            return Ok("Book borrowed successfully.");
        }

        [Authorize(Roles = "Student")]
        [HttpPost("{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            var username = User.Identity!.Name!;

            var memberId = _userService.GetMemberId(username);

            if (memberId == null)
            {
                _logger.LogWarning(
                    "Member not found for username {Username}",
                    username);

                return Unauthorized();
            }

            _logger.LogInformation(
                "Member {MemberId} is returning Book {BookId}",
                memberId,
                id);

            var success = _bookService.ReturnBook(id, memberId.Value);

            if (!success)
            {
                _logger.LogWarning(
                    "Return failed for Member {MemberId} Book {BookId}",
                    memberId,
                    id);

                return BadRequest("Return operation failed.");
            }

            _logger.LogInformation(
                "Return successful for Member {MemberId} Book {BookId}",
                memberId,
                id);

            return Ok("Book returned successfully.");
        }

        [Authorize(Roles = "Student")]
        [HttpGet("mybooks")]
        public IActionResult GetMyBorrowedBooks()
        {
            var username = User.Identity!.Name!;

            var memberId = _userService.GetMemberId(username);

            if (memberId == null)
            {
                _logger.LogWarning(
                    "Member not found for username {Username}",
                    username);

                return Unauthorized();
            }

            _logger.LogInformation(
                "Fetching currently borrowed books for Member {MemberId}",
                memberId);

            var books = _bookService.GetCurrentBorrowedBooks(memberId.Value);

            return Ok(books);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("history")]
        public IActionResult GetBorrowHistory()
        {
            var username = User.Identity!.Name!;

            var memberId = _userService.GetMemberId(username);

            if (memberId == null)
            {
                _logger.LogWarning(
                    "Member not found for username {Username}",
                    username);

                return Unauthorized();
            }

            _logger.LogInformation(
                "Fetching borrow history for Member {MemberId}",
                memberId);

            var history = _bookService.GetBorrowHistory(memberId.Value);

            return Ok(history);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/borrowrecords")]
        public IActionResult GetBorrowRecords(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 5)
        {
            var records =
                _bookService.GetBorrowRecords(pageNumber, pageSize);

            return Ok(records);
        }
    }
}