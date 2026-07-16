using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Services
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private AppDbContext _context = null!;

        private DashboardService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _service = new DashboardService(_context);
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
            int availableCopies = 5)
        {
            return new Book
            {
                Title = title,
                Author = "Robert C. Martin",
                PublishedYear = 2008,
                TotalCopies = availableCopies,
                AvailableCopies = availableCopies
            };
        }

        private Member CreateMember(
            string name = "Rinoy")
        {
            return new Member
            {
                Name = name,
                Email = $"{name.ToLower()}@test.com"
            };
        }

        //----------------------------------------------------------
        // Student Dashboard Tests
        //----------------------------------------------------------

        [Test]
        public void StudentDashboard_ShouldReturnCorrectAvailableBooks()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Book 1"),

                CreateBook("Book 2"),

                CreateBook("Book 3")

            );

            _context.SaveChanges();

            var member = CreateMember();

            _context.Members.Add(member);

            _context.SaveChanges();

            // Act

            var result = _service.GetStudentDashboard(member.Id);

            // Assert

            Assert.That(result.AvailableBooks, Is.EqualTo(3));
        }

        [Test]
        public void StudentDashboard_ShouldReturnCorrectBorrowedBooks()
        {
            // Arrange

            var member = CreateMember();

            var book = CreateBook();

            _context.Members.Add(member);

            _context.Books.Add(book);

            _context.SaveChanges();

            _context.BorrowRecords.Add(

                new BorrowRecord
                {
                    MemberId = member.Id,
                    BookId = book.Id,
                    BorrowDate = DateTime.UtcNow
                });

            _context.SaveChanges();

            // Act

            var result = _service.GetStudentDashboard(member.Id);

            // Assert

            Assert.That(result.BorrowedBooks, Is.EqualTo(1));
        }

        [Test]
        public void StudentDashboard_ShouldReturnCorrectReturnedBooks()
        {
            // Arrange

            var member = CreateMember();

            var book = CreateBook();

            _context.Members.Add(member);

            _context.Books.Add(book);

            _context.SaveChanges();

            _context.BorrowRecords.Add(

                new BorrowRecord
                {
                    MemberId = member.Id,
                    BookId = book.Id,
                    BorrowDate = DateTime.UtcNow.AddDays(-2),
                    ReturnDate = DateTime.UtcNow
                });

            _context.SaveChanges();

            // Act

            var result = _service.GetStudentDashboard(member.Id);

            // Assert

            Assert.That(result.ReturnedBooks, Is.EqualTo(1));
        }

        [Test]
        public void StudentDashboard_ShouldReturnCorrectTotalBorrowed()
        {
            // Arrange

            var member = CreateMember();

            _context.Members.Add(member);

            _context.SaveChanges();

            var book1 = CreateBook("Book1");

            var book2 = CreateBook("Book2");

            _context.Books.AddRange(book1, book2);

            _context.SaveChanges();

            _context.BorrowRecords.AddRange(

                new BorrowRecord
                {
                    MemberId = member.Id,
                    BookId = book1.Id,
                    BorrowDate = DateTime.UtcNow
                },

                new BorrowRecord
                {
                    MemberId = member.Id,
                    BookId = book2.Id,
                    BorrowDate = DateTime.UtcNow,
                    ReturnDate = DateTime.UtcNow
                });

            _context.SaveChanges();

            // Act

            var result = _service.GetStudentDashboard(member.Id);

            // Assert

            Assert.That(result.TotalBorrowed, Is.EqualTo(2));
        }

        //----------------------------------------------------------
        // Admin Dashboard Tests
        //----------------------------------------------------------

        [Test]
        public void AdminDashboard_ShouldReturnCorrectTotalBooks()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Book1"),

                CreateBook("Book2"),

                CreateBook("Book3")

            );

            _context.SaveChanges();

            // Act

            var result = _service.GetAdminDashboard();

            // Assert

            Assert.That(result.TotalBooks, Is.EqualTo(3));
        }

        [Test]
        public void AdminDashboard_ShouldReturnCorrectAvailableCopies()
        {
            // Arrange

            _context.Books.AddRange(

                CreateBook("Book1", 5),

                CreateBook("Book2", 2),

                CreateBook("Book3", 8)

            );

            _context.SaveChanges();

            // Act

            var result = _service.GetAdminDashboard();

            // Assert

            Assert.That(result.AvailableBooks, Is.EqualTo(15));
        }

        [Test]
        public void AdminDashboard_ShouldReturnCorrectBorrowedBooks()
        {
            // Arrange

            var member = CreateMember();

            var book = CreateBook();

            _context.Members.Add(member);

            _context.Books.Add(book);

            _context.SaveChanges();

            _context.BorrowRecords.Add(

                new BorrowRecord
                {
                    MemberId = member.Id,
                    BookId = book.Id,
                    BorrowDate = DateTime.UtcNow
                });

            _context.SaveChanges();

            // Act

            var result = _service.GetAdminDashboard();

            // Assert

            Assert.That(result.BorrowedBooks, Is.EqualTo(1));
        }

        [Test]
        public void AdminDashboard_ShouldReturnCorrectStudentCount()
        {
            // Arrange

            _context.Members.AddRange(

                CreateMember("Rinoy"),

                CreateMember("John"),

                CreateMember("Alice")

            );

            _context.SaveChanges();

            // Act

            var result = _service.GetAdminDashboard();

            // Assert

            Assert.That(result.Students, Is.EqualTo(3));
        }
    }
}