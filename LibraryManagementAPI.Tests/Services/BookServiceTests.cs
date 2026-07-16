using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private AppDbContext _context = null!;

        private BookService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _service = new BookService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();
        }

        //----------------------------------------------------------
        // Helper Methods
        //----------------------------------------------------------

        private Book CreateBook(
            string title = "Clean Code",
            string author = "Robert C. Martin",
            int year = 2008,
            int totalCopies = 5,
            int availableCopies = 5)
        {
            return new Book
            {
                Title = title,
                Author = author,
                PublishedYear = year,
                TotalCopies = totalCopies,
                AvailableCopies = availableCopies
            };
        }

        private Member CreateMember(
            string name = "Rinoy Joy",
            string email = "rinoy@test.com")
        {
            return new Member
            {
                Name = name,
                Email = email
            };
        }

        //----------------------------------------------------------
        // GetBookCount Tests
        //----------------------------------------------------------

        [Test]
        public void GetBookCount_ShouldReturnZero_WhenDatabaseIsEmpty()
        {
            // Act

            int result = _service.GetBookCount();

            // Assert

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void GetBookCount_ShouldReturnCorrectCount()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook(),

                CreateBook(
                    "CLR via C#",
                    "Jeffrey Richter",
                    2012,
                    3,
                    3)

            );

            _context.SaveChanges();

            // Act

            int result = _service.GetBookCount();

            // Assert

            Assert.That(result, Is.EqualTo(2));
        }

        //----------------------------------------------------------
        // AddBook Tests
        //----------------------------------------------------------

        [Test]
        public void AddBook_ShouldCreateBook()
        {
            // Arrange

            var request = new AddBookRequest
            {
                Title = "The Pragmatic Programmer",
                Author = "Andy Hunt",
                PublishedYear = 1999,
                TotalCopies = 10
            };

            // Act

            var result = _service.AddBook(request);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result.Title, Is.EqualTo(request.Title));

            Assert.That(_context.Books.Count(), Is.EqualTo(1));
        }

        [Test]
        public void AddBook_ShouldSetAvailableCopiesEqualToTotalCopies()
        {
            // Arrange

            var request = new AddBookRequest
            {
                Title = "Head First C#",
                Author = "Andrew Stellman",
                PublishedYear = 2021,
                TotalCopies = 7
            };

            // Act

            var result = _service.AddBook(request);

            // Assert

            Assert.That(result.AvailableCopies, Is.EqualTo(7));
        }

        //----------------------------------------------------------
        // GetBookById Tests
        //----------------------------------------------------------

        [Test]
        public void GetBookById_ShouldReturnBook_WhenBookExists()
        {
            // Arrange

            var book = CreateBook();

            _context.Books.Add(book);

            _context.SaveChanges();

            // Act

            var result = _service.GetBookById(book.Id);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Title, Is.EqualTo(book.Title));

            Assert.That(result.Author, Is.EqualTo(book.Author));
        }

        [Test]
        public void GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Act

            var result = _service.GetBookById(999);

            // Assert

            Assert.That(result, Is.Null);
        }

        //----------------------------------------------------------
        // UpdateBook Tests
        //----------------------------------------------------------

        [Test]
        public void UpdateBook_ShouldUpdateExistingBook()
        {
            // Arrange

            var book = CreateBook();

            _context.Books.Add(book);

            _context.SaveChanges();

            var request = new UpdateBookRequest
            {
                Title = "Updated Clean Code",
                Author = "Uncle Bob",
                PublishedYear = 2020,
                TotalCopies = 10,
                AvailableCopies = 8
            };

            // Act

            var result = _service.UpdateBook(book.Id, request);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Title, Is.EqualTo(request.Title));

            Assert.That(result.Author, Is.EqualTo(request.Author));

            Assert.That(result.PublishedYear, Is.EqualTo(request.PublishedYear));

            Assert.That(result.TotalCopies, Is.EqualTo(request.TotalCopies));

            Assert.That(result.AvailableCopies, Is.EqualTo(request.AvailableCopies));
        }

        [Test]
        public void UpdateBook_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange

            var request = new UpdateBookRequest
            {
                Title = "Book",
                Author = "Author",
                PublishedYear = 2024,
                TotalCopies = 5,
                AvailableCopies = 5
            };

            // Act

            var result = _service.UpdateBook(999, request);

            // Assert

            Assert.That(result, Is.Null);
        }

        //----------------------------------------------------------
        // DeleteBook Tests
        //----------------------------------------------------------

        [Test]
        public void DeleteBook_ShouldDeleteExistingBook()
        {
            // Arrange

            var book = CreateBook();

            _context.Books.Add(book);

            _context.SaveChanges();

            // Act

            bool result = _service.DeleteBook(book.Id);

            // Assert

            Assert.That(result, Is.True);

            Assert.That(_context.Books.Count(), Is.EqualTo(0));
        }

        [Test]
        public void DeleteBook_ShouldReturnFalse_WhenBookDoesNotExist()
        {
            // Act

            bool result = _service.DeleteBook(999);

            // Assert

            Assert.That(result, Is.False);
        }

        //----------------------------------------------------------
        // SearchBooks Tests
        //----------------------------------------------------------

        [Test]
        public void SearchBooks_ShouldReturnMatchingBooks()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Clean Code"),

                CreateBook("CLR via C#"),

                CreateBook("Design Patterns")

            );

            _context.SaveChanges();

            // Act

            var result = _service.SearchBooks("Clean");

            // Assert

            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result[0].Title, Is.EqualTo("Clean Code"));
        }

        [Test]
        public void SearchBooks_ShouldReturnEmptyList_WhenNoBookMatches()
        {
            // Arrange

            _context.Books.Add(CreateBook());

            _context.SaveChanges();

            // Act

            var result = _service.SearchBooks("Python");

            // Assert

            Assert.That(result, Is.Empty);
        }

        //----------------------------------------------------------
        // GetBooksSortedByYear Tests
        //----------------------------------------------------------

        [Test]
        public void GetBooksSortedByYear_ShouldReturnBooksInAscendingOrder()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Book 2020", "Author", 2020),

                CreateBook("Book 2010", "Author", 2010),

                CreateBook("Book 2015", "Author", 2015)

            );

            _context.SaveChanges();

            // Act

            var result = _service.GetBooksSortedByYear();

            // Assert

            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result[0].PublishedYear, Is.EqualTo(2010));

            Assert.That(result[1].PublishedYear, Is.EqualTo(2015));

            Assert.That(result[2].PublishedYear, Is.EqualTo(2020));
        }

        //----------------------------------------------------------
        // BorrowBook Tests
        //----------------------------------------------------------

        [Test]
        public void BorrowBook_ShouldReturnTrue_WhenBookIsAvailable()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            bool result = _service.BorrowBook(book.Id, member.Id);

            // Assert

            Assert.That(result, Is.True);
        }

        [Test]
        public void BorrowBook_ShouldDecreaseAvailableCopies()
        {
            // Arrange

            var book = CreateBook(
                totalCopies: 5,
                availableCopies: 5);

            var member = CreateMember();

            _context.Books.Add(book);

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            _service.BorrowBook(book.Id, member.Id);

            // Assert

            var updatedBook = _context.Books.Find(book.Id);

            Assert.That(updatedBook, Is.Not.Null);

            Assert.That(updatedBook!.AvailableCopies, Is.EqualTo(4));
        }

        [Test]
        public void BorrowBook_ShouldCreateBorrowRecord()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            _service.BorrowBook(book.Id, member.Id);

            // Assert

            Assert.That(_context.BorrowRecords.Count(), Is.EqualTo(1));

            var record = _context.BorrowRecords.First();

            Assert.That(record.BookId, Is.EqualTo(book.Id));

            Assert.That(record.MemberId, Is.EqualTo(member.Id));

            Assert.That(record.ReturnDate, Is.Null);
        }

        [Test]
        public void BorrowBook_ShouldReturnFalse_WhenBookDoesNotExist()
        {
            // Arrange

            var member = CreateMember();

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            bool result = _service.BorrowBook(999, member.Id);

            // Assert

            Assert.That(result, Is.False);
        }

        [Test]
        public void BorrowBook_ShouldReturnFalse_WhenMemberDoesNotExist()
        {
            // Arrange

            var book = CreateBook();

            _context.Books.Add(book);

            _context.SaveChanges();

            // Act

            bool result = _service.BorrowBook(book.Id, 999);

            // Assert

            Assert.That(result, Is.False);
        }

        [Test]
        public void BorrowBook_ShouldReturnFalse_WhenNoCopiesAreAvailable()
        {
            // Arrange

            var book = CreateBook(
                totalCopies: 5,
                availableCopies: 0);

            var member = CreateMember();

            _context.Books.Add(book);

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            bool result = _service.BorrowBook(book.Id, member.Id);

            // Assert

            Assert.That(result, Is.False);

            Assert.That(_context.BorrowRecords.Count(), Is.EqualTo(0));

            Assert.That(book.AvailableCopies, Is.EqualTo(0));
        }

        //----------------------------------------------------------
        // ReturnBook Tests
        //----------------------------------------------------------

        [Test]
        public void ReturnBook_ShouldReturnTrue_WhenBorrowRecordExists()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            // Act

            bool result = _service.ReturnBook(book.Id, member.Id);

            // Assert

            Assert.That(result, Is.True);
        }

        [Test]
        public void ReturnBook_ShouldIncreaseAvailableCopies()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            // Act

            _service.ReturnBook(book.Id, member.Id);

            // Assert

            var updatedBook = _context.Books.Find(book.Id);

            Assert.That(updatedBook!.AvailableCopies, Is.EqualTo(5));
        }

        [Test]
        public void ReturnBook_ShouldSetReturnDate()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            // Act

            _service.ReturnBook(book.Id, member.Id);

            // Assert

            var record = _context.BorrowRecords.First();

            Assert.That(record.ReturnDate, Is.Not.Null);
        }

        [Test]
        public void ReturnBook_ShouldReturnFalse_WhenBorrowRecordDoesNotExist()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            bool result = _service.ReturnBook(book.Id, member.Id);

            // Assert

            Assert.That(result, Is.False);
        }

        //----------------------------------------------------------
        // GetCurrentBorrowedBooks Tests
        //----------------------------------------------------------

        [Test]
        public void GetCurrentBorrowedBooks_ShouldReturnBorrowedBooks()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            // Act

            var result = _service.GetCurrentBorrowedBooks(
                member.Id,
                1,
                10);

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(1));

            Assert.That(result.Items[0].BookTitle, Is.EqualTo(book.Title));
        }

        //----------------------------------------------------------
        // GetBorrowHistory Tests
        //----------------------------------------------------------

        [Test]
        public void GetBorrowHistory_ShouldIncludeReturnedBooks()
        {
            // Arrange

            var book = CreateBook();

            var member = CreateMember();

            _context.Books.Add(book);
            _context.Members.Add(member);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            _service.ReturnBook(book.Id, member.Id);

            // Act

            var result = _service.GetBorrowHistory(
                member.Id,
                1,
                10);

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(1));

            Assert.That(result.Items[0].Status, Is.EqualTo("Returned"));
        }

        //----------------------------------------------------------
        // Pagination Tests
        //----------------------------------------------------------

        [Test]
        public void GetAllBooks_ShouldReturnCorrectPageSize()
        {
            // Arrange

            for (int i = 1; i <= 20; i++)
            {
                _context.Books.Add(
                    CreateBook($"Book {i}")
                );
            }

            _context.SaveChanges();

            // Act

            var result = _service.GetAllBooks(
                2,
                5);

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(5));

            Assert.That(result.CurrentPage, Is.EqualTo(2));

            Assert.That(result.TotalPages, Is.EqualTo(4));
        }

        //----------------------------------------------------------
        // Search Tests
        //----------------------------------------------------------

        [Test]
        public void GetAllBooks_ShouldSearchByTitle()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Clean Code"),

                CreateBook("CLR via C#"),

                CreateBook("Design Patterns")

            );

            _context.SaveChanges();

            // Act

            var result = _service.GetAllBooks(
                1,
                10,
                "Clean");

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(1));

            Assert.That(result.Items[0].Title, Is.EqualTo("Clean Code"));
        }

        [Test]
        public void GetCurrentBorrowedBooks_ShouldSearchBorrowedBooks()
        {
            // Arrange

            var member = CreateMember();

            var clean = CreateBook("Clean Code");

            var clr = CreateBook("CLR via C#");

            _context.Members.Add(member);

            _context.Books.AddRange(clean, clr);

            _context.SaveChanges();

            _service.BorrowBook(clean.Id, member.Id);

            _service.BorrowBook(clr.Id, member.Id);

            // Act

            var result = _service.GetCurrentBorrowedBooks(
                member.Id,
                1,
                10,
                "Clean");

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(1));

            Assert.That(result.Items[0].BookTitle, Is.EqualTo("Clean Code"));
        }

        //----------------------------------------------------------
        // Admin Borrow Records
        //----------------------------------------------------------

        [Test]
        public void GetBorrowRecords_ShouldReturnAllBorrowRecords()
        {
            // Arrange

            var member = CreateMember();

            var book = CreateBook();

            _context.Members.Add(member);

            _context.Books.Add(book);

            _context.SaveChanges();

            _service.BorrowBook(book.Id, member.Id);

            // Act

            var result = _service.GetBorrowRecords(
                1,
                10);

            // Assert

            Assert.That(result.Items.Count, Is.EqualTo(1));

            Assert.That(result.TotalRecords, Is.EqualTo(1));
        }
    }
}