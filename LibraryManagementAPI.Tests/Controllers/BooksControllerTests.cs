using System.Security.Claims;

using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBookService> _bookServiceMock = null!;

        private Mock<IUserService> _userServiceMock = null!;

        private Mock<ILogger<BooksController>> _loggerMock = null!;

        private BooksController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _bookServiceMock =
                new Mock<IBookService>();

            _userServiceMock =
                new Mock<IUserService>();

            _loggerMock =
                new Mock<ILogger<BooksController>>();

            _controller =
                new BooksController(

                    _bookServiceMock.Object,

                    _userServiceMock.Object,

                    _loggerMock.Object
                );
        }

        //----------------------------------------------------------
        // Helper Methods
        //----------------------------------------------------------

        private BookResponse CreateBook()
        {
            return new BookResponse
            {
                Id = 1,

                Title = "Clean Code",

                Author = "Robert C. Martin",

                PublishedYear = 2008,

                TotalCopies = 5,

                AvailableCopies = 5
            };
        }

        private AddBookRequest CreateAddBook()
        {
            return new AddBookRequest
            {
                Title = "Clean Code",

                Author = "Robert C. Martin",

                PublishedYear = 2008,

                TotalCopies = 5
            };
        }

        private UpdateBookRequest CreateUpdateBook()
        {
            return new UpdateBookRequest
            {
                Title = "Updated Book",

                Author = "Updated Author",

                PublishedYear = 2024,

                TotalCopies = 10,

                AvailableCopies = 9
            };
        }

        private void SetUser(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity =
                new ClaimsIdentity(claims);

            var principal =
                new ClaimsPrincipal(identity);

            _controller.ControllerContext =
                new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal
                    }
                };
        }

        //----------------------------------------------------------
        // GetBooks Tests
        //----------------------------------------------------------

        [Test]
        public void GetBooks_ShouldReturnOk()
        {
            // Arrange

            var response =
                new PagedResponse<BookResponse>
                {
                    CurrentPage = 1,

                    TotalPages = 1,

                    TotalRecords = 1,

                    PageSize = 5,

                    Items =
                    [
                        CreateBook()
                    ]
                };

            _bookServiceMock

                .Setup(x => x.GetAllBooks(1, 5, null))

                .Returns(response);

            // Act

            var result =
                _controller.GetBooks();

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo(response));
        }

        //----------------------------------------------------------
        // GetBookCount Tests
        //----------------------------------------------------------

        [Test]
        public void GetBookCount_ShouldReturnCorrectCount()
        {
            // Arrange

            _bookServiceMock

                .Setup(x => x.GetBookCount())

                .Returns(15);

            // Act

            var result =
                _controller.GetBooksCount();

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo(15));
        }

        //----------------------------------------------------------
        // GetBookById Tests
        //----------------------------------------------------------

        [Test]
        public void GetBookById_ShouldReturnOk_WhenBookExists()
        {
            // Arrange

            var book =
                CreateBook();

            _bookServiceMock

                .Setup(x => x.GetBookById(1))

                .Returns(book);

            // Act

            var result =
                _controller.GetBookById(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo(book));
        }

        [Test]
        public void GetBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange

            _bookServiceMock

                .Setup(x => x.GetBookById(999))

                .Returns((BookResponse?)null);

            // Act

            var result =
                _controller.GetBookById(999);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<NotFoundResult>());
        }

        //----------------------------------------------------------
        // AddBook Tests
        //----------------------------------------------------------

        [Test]
        public void AddBook_ShouldReturnCreatedAtAction()
        {
            // Arrange

            var request = CreateAddBook();

            var response = CreateBook();

            _bookServiceMock

                .Setup(x => x.AddBook(It.IsAny<AddBookRequest>()))

                .Returns(response);

            // Act

            var result = _controller.AddBook(request);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<CreatedAtActionResult>());

            var created =
                result as CreatedAtActionResult;

            Assert.That(
                created!.Value,
                Is.EqualTo(response));

            Assert.That(
                created.ActionName,
                Is.EqualTo(nameof(_controller.GetBookById)));

            _bookServiceMock.Verify(

                x => x.AddBook(It.IsAny<AddBookRequest>()),

                Times.Once);
        }

        //----------------------------------------------------------
        // UpdateBook Tests
        //----------------------------------------------------------

        [Test]
        public void UpdateBook_ShouldReturnOk_WhenBookExists()
        {
            // Arrange

            var request = CreateUpdateBook();

            var response = CreateBook();

            _bookServiceMock

                .Setup(x => x.UpdateBook(1, It.IsAny<UpdateBookRequest>()))

                .Returns(response);

            // Act

            var result =
                _controller.UpdateBook(1, request);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            _bookServiceMock.Verify(

                x => x.UpdateBook(
                    1,
                    It.IsAny<UpdateBookRequest>()),

                Times.Once);
        }

        [Test]
        public void UpdateBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange

            var request = CreateUpdateBook();

            _bookServiceMock

                .Setup(x => x.UpdateBook(999, It.IsAny<UpdateBookRequest>()))

                .Returns((BookResponse?)null);

            // Act

            var result =
                _controller.UpdateBook(999, request);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<NotFoundResult>());
        }

        //----------------------------------------------------------
        // DeleteBook Tests
        //----------------------------------------------------------

        [Test]
        public void DeleteBook_ShouldReturnNoContent_WhenDeleteSucceeds()
        {
            // Arrange

            _bookServiceMock

                .Setup(x => x.DeleteBook(1))

                .Returns(true);

            // Act

            var result =
                _controller.DeleteBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<NoContentResult>());

            _bookServiceMock.Verify(

                x => x.DeleteBook(1),

                Times.Once);
        }

        [Test]
        public void DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange

            _bookServiceMock

                .Setup(x => x.DeleteBook(999))

                .Returns(false);

            // Act

            var result =
                _controller.DeleteBook(999);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<NotFoundResult>());
        }

        //----------------------------------------------------------
        // BorrowBook Tests
        //----------------------------------------------------------

        [Test]
        public void BorrowBook_ShouldReturnOk_WhenBorrowSucceeds()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            _bookServiceMock

                .Setup(x => x.BorrowBook(1, 1))

                .Returns(true);

            // Act

            var result =
                _controller.BorrowBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo("Book borrowed successfully."));

            _bookServiceMock.Verify(

                x => x.BorrowBook(1, 1),

                Times.Once);
        }

        [Test]
        public void BorrowBook_ShouldReturnBadRequest_WhenBorrowFails()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            _bookServiceMock

                .Setup(x => x.BorrowBook(1, 1))

                .Returns(false);

            // Act

            var result =
                _controller.BorrowBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<BadRequestObjectResult>());

            var badRequest =
                result as BadRequestObjectResult;

            Assert.That(
                badRequest!.Value,
                Is.EqualTo("Borrow operation failed."));
        }

        [Test]
        public void BorrowBook_ShouldReturnUnauthorized_WhenMemberDoesNotExist()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns((int?)null);

            // Act

            var result =
                _controller.BorrowBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<UnauthorizedResult>());
        }

        //----------------------------------------------------------
        // ReturnBook Tests
        //----------------------------------------------------------

        [Test]
        public void ReturnBook_ShouldReturnOk_WhenReturnSucceeds()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            _bookServiceMock

                .Setup(x => x.ReturnBook(1, 1))

                .Returns(true);

            // Act

            var result =
                _controller.ReturnBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo("Book returned successfully."));

            _bookServiceMock.Verify(

                x => x.ReturnBook(1, 1),

                Times.Once);
        }

        [Test]
        public void ReturnBook_ShouldReturnBadRequest_WhenReturnFails()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            _bookServiceMock

                .Setup(x => x.ReturnBook(1, 1))

                .Returns(false);

            // Act

            var result =
                _controller.ReturnBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<BadRequestObjectResult>());

            var badRequest =
                result as BadRequestObjectResult;

            Assert.That(
                badRequest!.Value,
                Is.EqualTo("Return operation failed."));
        }

        [Test]
        public void ReturnBook_ShouldReturnUnauthorized_WhenMemberDoesNotExist()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns((int?)null);

            // Act

            var result =
                _controller.ReturnBook(1);

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<UnauthorizedResult>());
        }

        //----------------------------------------------------------
        // SearchBooks Tests
        //----------------------------------------------------------

        [Test]
        public void SearchBooks_ShouldReturnOk()
        {
            // Arrange

            var books = new List<BookResponse>
            {
                CreateBook()
            };

            _bookServiceMock

                .Setup(x => x.SearchBooks("Clean"))

                .Returns(books);

            // Act

            var result = _controller.SearchBooks("Clean");

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(books));
        }

        //----------------------------------------------------------
        // Sorted Books Tests
        //----------------------------------------------------------

        [Test]
        public void GetSortedBooks_ShouldReturnOk()
        {
            // Arrange

            var books = new List<BookResponse>
            {
                CreateBook()
            };

            _bookServiceMock

                .Setup(x => x.GetBooksSortedByYear())

                .Returns(books);

            // Act

            var result = _controller.GetSortedBooks();

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(books));
        }

        //----------------------------------------------------------
        // My Books Tests
        //----------------------------------------------------------

        [Test]
        public void GetMyBorrowedBooks_ShouldReturnOk_WhenMemberExists()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            var response = new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalPages = 1,
                TotalRecords = 1,
                Items = new List<BorrowRecordResponse>()
            };

            _bookServiceMock

                .Setup(x => x.GetCurrentBorrowedBooks(
                    1,
                    1,
                    5,
                    null))

                .Returns(response);

            // Act

            var result = _controller.GetMyBorrowedBooks();

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(response));
        }

        [Test]
        public void GetMyBorrowedBooks_ShouldReturnUnauthorized_WhenMemberDoesNotExist()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns((int?)null);

            // Act

            var result = _controller.GetMyBorrowedBooks();

            // Assert

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        //----------------------------------------------------------
        // Borrow History Tests
        //----------------------------------------------------------

        [Test]
        public void GetBorrowHistory_ShouldReturnOk_WhenMemberExists()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            var response = new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalPages = 1,
                TotalRecords = 1,
                Items = new List<BorrowRecordResponse>()
            };

            _bookServiceMock

                .Setup(x => x.GetBorrowHistory(
                    1,
                    1,
                    5,
                    null))

                .Returns(response);

            // Act

            var result = _controller.GetBorrowHistory();

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(response));
        }

        [Test]
        public void GetBorrowHistory_ShouldReturnUnauthorized_WhenMemberDoesNotExist()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns((int?)null);

            // Act

            var result = _controller.GetBorrowHistory();

            // Assert

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        //----------------------------------------------------------
        // Admin Borrow Records Tests
        //----------------------------------------------------------

        [Test]
        public void GetBorrowRecords_ShouldReturnOk()
        {
            // Arrange

            var response = new PagedResponse<BorrowRecordResponse>
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalPages = 1,
                TotalRecords = 1,
                Items = new List<BorrowRecordResponse>()
            };

            _bookServiceMock

                .Setup(x => x.GetBorrowRecords(
                    1,
                    5,
                    null,
                    null))

                .Returns(response);

            // Act

            var result = _controller.GetBorrowRecords();

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(response));
        }
    }
}