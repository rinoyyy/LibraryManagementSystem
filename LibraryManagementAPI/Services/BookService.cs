using LibraryManagementAPI.Data;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }

        public Book AddBook(Book book)
        {
            _context.Books.Add(book);

            _context.SaveChanges();

            return book;
        }

        public int GetBookCount()
        {
            return _context.Books.Count();
        }

        public List<Book> SearchBooks(string keyword)
        {
            return _context.Books
                           .Where(b => b.Title.Contains(keyword))
                           .ToList();
        }

        public List<Book> GetBooksSortedByYear()
        {
            return _context.Books
                           .OrderBy(b => b.PublishedYear)
                           .ToList();
        }

        public Book? UpdateBook(int id, Book updatedBook)
        {
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.TotalCopies = updatedBook.TotalCopies;
            existingBook.AvailableCopies = updatedBook.AvailableCopies;

            _context.SaveChanges();

            return existingBook;
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);

            _context.SaveChanges();

            return true;
        }
        public bool BorrowBook(int bookId, int memberId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                return false;
            }

            var member = _context.Members.FirstOrDefault(m => m.Id == memberId);

            if (member == null)
            {
                return false;
            }

            if (book.AvailableCopies <= 0)
            {
                return false;
            }

            var borrowRecord = new BorrowRecord
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.UtcNow
            };

            _context.BorrowRecords.Add(borrowRecord);

            book.AvailableCopies--;

            _context.SaveChanges();

            return true;
        }

        public bool ReturnBook(int bookId, int memberId)
        {
            var borrowRecord = _context.BorrowRecords
                .FirstOrDefault(br =>
                    br.BookId == bookId &&
                    br.MemberId == memberId &&
                    br.ReturnDate == null);

            if (borrowRecord == null)
            {
                return false;
            }

            var book = _context.Books
                .FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                return false;
            }

            borrowRecord.ReturnDate = DateTime.UtcNow;

            book.AvailableCopies++;

            _context.SaveChanges();

            return true;
        }
    }
}