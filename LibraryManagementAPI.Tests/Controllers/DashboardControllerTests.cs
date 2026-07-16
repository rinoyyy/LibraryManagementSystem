using System.Security.Claims;

using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Controllers
{
    [TestFixture]
    public class DashboardControllerTests
    {
        private Mock<IDashboardService> _dashboardServiceMock = null!;

        private Mock<IUserService> _userServiceMock = null!;

        private DashboardController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _dashboardServiceMock =
                new Mock<IDashboardService>();

            _userServiceMock =
                new Mock<IUserService>();

            _controller =
                new DashboardController(

                    _dashboardServiceMock.Object,

                    _userServiceMock.Object);
        }

        //----------------------------------------------------------
        // Helper
        //----------------------------------------------------------

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
        // Student Dashboard Tests
        //----------------------------------------------------------

        [Test]
        public void GetStudentDashboard_ShouldReturnOk_WhenMemberExists()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns(1);

            var response = new StudentDashboardResponse
            {
                AvailableBooks = 20,

                BorrowedBooks = 2,

                ReturnedBooks = 5,

                TotalBorrowed = 7
            };

            _dashboardServiceMock

                .Setup(x => x.GetStudentDashboard(1))

                .Returns(response);

            // Act

            var result =
                _controller.GetStudentDashboard();

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo(response));

            _dashboardServiceMock.Verify(

                x => x.GetStudentDashboard(1),

                Times.Once);
        }

        [Test]
        public void GetStudentDashboard_ShouldReturnUnauthorized_WhenMemberDoesNotExist()
        {
            // Arrange

            SetUser("rinoy");

            _userServiceMock

                .Setup(x => x.GetMemberId("rinoy"))

                .Returns((int?)null);

            // Act

            var result =
                _controller.GetStudentDashboard();

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<UnauthorizedResult>());
        }

        //----------------------------------------------------------
        // Admin Dashboard Tests
        //----------------------------------------------------------

        [Test]
        public void GetAdminDashboard_ShouldReturnOk()
        {
            // Arrange

            var response = new AdminDashboardResponse
            {
                TotalBooks = 25,

                AvailableBooks = 18,

                BorrowedBooks = 7,

                Students = 10
            };

            _dashboardServiceMock

                .Setup(x => x.GetAdminDashboard())

                .Returns(response);

            // Act

            var result =
                _controller.GetAdminDashboard();

            // Assert

            Assert.That(
                result,
                Is.InstanceOf<OkObjectResult>());

            var ok =
                result as OkObjectResult;

            Assert.That(
                ok!.Value,
                Is.EqualTo(response));

            _dashboardServiceMock.Verify(

                x => x.GetAdminDashboard(),

                Times.Once);
        }

        [Test]
        public void GetAdminDashboard_ShouldCallServiceOnce()
        {
            // Arrange

            _dashboardServiceMock

                .Setup(x => x.GetAdminDashboard())

                .Returns(new AdminDashboardResponse());

            // Act

            _controller.GetAdminDashboard();

            // Assert

            _dashboardServiceMock.Verify(

                x => x.GetAdminDashboard(),

                Times.Once);
        }
    }
}