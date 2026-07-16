using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock = null!;

        private AuthController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _authServiceMock =
                new Mock<IAuthService>();

            _controller =
                new AuthController(
                    _authServiceMock.Object);
        }

        //----------------------------------------------------------
        // Helper Methods
        //----------------------------------------------------------

        private RegisterRequest CreateStudent()
        {
            return new RegisterRequest
            {
                Username = "rinoy",

                Password = "Password123",

                Name = "Rinoy Joy",

                Email = "rinoy@test.com"
            };
        }

        private LoginRequest CreateLogin()
        {
            return new LoginRequest
            {
                Username = "rinoy",

                Password = "Password123"
            };
        }

        //----------------------------------------------------------
        // RegisterStudent Tests
        //----------------------------------------------------------

        [Test]
        public void RegisterStudent_ShouldReturnOk_WhenRegistrationSucceeds()
        {
            // Arrange

            _authServiceMock

                .Setup(x => x.RegisterStudent(It.IsAny<RegisterRequest>()))

                .Returns(true);

            var request = CreateStudent();

            // Act

            var result = _controller.RegisterStudent(request);

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value,
                Is.EqualTo("Student registered successfully."));
        }

        [Test]
        public void RegisterStudent_ShouldReturnBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange

            _authServiceMock

                .Setup(x => x.RegisterStudent(It.IsAny<RegisterRequest>()))

                .Returns(false);

            var request = CreateStudent();

            // Act

            var result = _controller.RegisterStudent(request);

            // Assert

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            var badRequest = result as BadRequestObjectResult;

            Assert.That(badRequest!.Value,
                Is.EqualTo("Username already exists."));
        }

        //----------------------------------------------------------
        // RegisterAdmin Tests
        //----------------------------------------------------------

        [Test]
        public void RegisterAdmin_ShouldReturnOk_WhenRegistrationSucceeds()
        {
            // Arrange

            _authServiceMock

                .Setup(x => x.RegisterAdmin(It.IsAny<RegisterRequest>()))

                .Returns(true);

            var request = CreateStudent();

            // Act

            var result = _controller.RegisterAdmin(request);

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value,
                Is.EqualTo("Admin registered successfully."));
        }

        [Test]
        public void RegisterAdmin_ShouldReturnBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange

            _authServiceMock

                .Setup(x => x.RegisterAdmin(It.IsAny<RegisterRequest>()))

                .Returns(false);

            var request = CreateStudent();

            // Act

            var result = _controller.RegisterAdmin(request);

            // Assert

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            var badRequest = result as BadRequestObjectResult;

            Assert.That(badRequest!.Value,
                Is.EqualTo("Username already exists."));
        }

        //----------------------------------------------------------
        // Login Tests
        //----------------------------------------------------------

        [Test]
        public void Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange

            var response = new LoginResponse
            {
                Username = "rinoy",

                Role = "Student",

                Token = "DummyJwtToken"
            };

            _authServiceMock

                .Setup(x => x.Login(It.IsAny<LoginRequest>()))

                .Returns(response);

            var request = CreateLogin();

            // Act

            var result = _controller.Login(request);

            // Assert

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;

            Assert.That(ok!.Value, Is.EqualTo(response));
        }

        [Test]
        public void Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange

            _authServiceMock

                .Setup(x => x.Login(It.IsAny<LoginRequest>()))

                .Returns((LoginResponse?)null);

            var request = CreateLogin();

            // Act

            var result = _controller.Login(request);

            // Assert

            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());

            var unauthorized = result as UnauthorizedObjectResult;

            Assert.That(unauthorized!.Value,
                Is.EqualTo("Invalid username or password."));
        }
    }
}

