using LibraryManagementAPI.Data;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public PagedResponse<BookResponse> GetAllBooks(
    int pageNumber,
    int pageSize,
    string? search = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Author.ToLower().Contains(search));
            }

            query = query.OrderBy(b => b.Id);

            var totalRecords = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    PublishedYear = b.PublishedYear,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies
                })
                .ToList();

            return new PagedResponse<BookResponse>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Items = items
            };
        }

        public BookResponse? GetBookById(int id)
        {
            return _context.Books
                .Where(book => book.Id == id)
                .Select(book => new BookResponse
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies
                })
                .FirstOrDefault();
        }

        public BookResponse AddBook(AddBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                PublishedYear = request.PublishedYear,
                TotalCopies = request.TotalCopies,

                // New books have all copies available
                AvailableCopies = request.TotalCopies
            };

            _context.Books.Add(book);

            _context.SaveChanges();

            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
        }

        public int GetBookCount()
        {
            return _context.Books.Count();
        }

        public List<BookResponse> SearchBooks(string keyword)
        {
            return _context.Books
                .Where(book => book.Title.Contains(keyword))
                .Select(book => new BookResponse
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies
                })
                .ToList();
        }

        public List<BookResponse> GetBooksSortedByYear()
        {
            return _context.Books
                .OrderBy(book => book.PublishedYear)
                .Select(book => new BookResponse
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies
                })
                .ToList();
        }

        public BookResponse? UpdateBook(int id, UpdateBookRequest request)
        {
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = request.Title;
            existingBook.Author = request.Author;
            existingBook.PublishedYear = request.PublishedYear;
            existingBook.TotalCopies = request.TotalCopies;
            existingBook.AvailableCopies = request.AvailableCopies;

            _context.SaveChanges();

            return new BookResponse
            {
                Id = existingBook.Id,
                Title = existingBook.Title,
                Author = existingBook.Author,
                PublishedYear = existingBook.PublishedYear,
                TotalCopies = existingBook.TotalCopies,
                AvailableCopies = existingBook.AvailableCopies
            };
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

        public PagedResponse<BorrowRecordResponse> GetCurrentBorrowedBooks(
    int memberId,
    int pageNumber,
    int pageSize,
    string? search = null)
        {
            var query = _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br =>
                    br.MemberId == memberId &&
                    br.ReturnDate == null)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(br =>
                    br.Book!.Title.ToLower().Contains(search) ||
                    br.Book.Author.ToLower().Contains(search));
            }

            query = query.OrderByDescending(br => br.BorrowDate);

            var totalRecords = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(br => new BorrowRecordResponse
                {
                    BorrowRecordId = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book!.Title,
                    StudentName = br.Member!.Name,
                    BorrowDate = br.BorrowDate,
                    ReturnDate = br.ReturnDate,
                    Status = "Borrowed"
                })
                .ToList();

            return new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Items = items
            };
        }

        public PagedResponse<BorrowRecordResponse> GetBorrowHistory(
    int memberId,
    int pageNumber,
    int pageSize,
    string? search = null)
        {
            var query = _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => br.MemberId == memberId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(br =>
                    br.Book!.Title.ToLower().Contains(search) ||
                    br.Book.Author.ToLower().Contains(search));
            }

            query = query.OrderByDescending(br => br.BorrowDate);

            var totalRecords = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(br => new BorrowRecordResponse
                {
                    BorrowRecordId = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book!.Title,
                    StudentName = br.Member!.Name,
                    BorrowDate = br.BorrowDate,
                    ReturnDate = br.ReturnDate,
                    Status = br.ReturnDate == null
                        ? "Borrowed"
                        : "Returned"
                })
                .ToList();

            return new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Items = items
            };
        }

        public PagedResponse<BorrowRecordResponse> GetBorrowRecords(
    int pageNumber,
    int pageSize,
    string? search = null)
        {
            var query = _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(br =>

                    br.Member!.Name.ToLower().Contains(search)

                    ||

                    br.Book!.Title.ToLower().Contains(search)

                    ||

                    br.Book.Author.ToLower().Contains(search)

                    ||

                    br.BorrowDate.ToString().ToLower().Contains(search)

                );
            }

            query = query.OrderByDescending(br => br.BorrowDate);

            var totalRecords = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(br => new BorrowRecordResponse
                {
                    BorrowRecordId = br.Id,
                    BookId = br.BookId,
                    BookTitle = br.Book!.Title,
                    StudentName = br.Member!.Name,
                    BorrowDate = br.BorrowDate,
                    ReturnDate = br.ReturnDate,
                    Status = br.ReturnDate == null
                        ? "Borrowed"
                        : "Returned"
                })
                .ToList();

            return new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Items = items
            };
        }
    }
}