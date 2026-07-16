using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using NUnit.Framework;

namespace LibraryManagementAPI.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AppDbContext _context = null!;

        private AuthService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var configuration = new ConfigurationBuilder()

                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsMyVerySecretKeyForTesting123456789" },
                    { "Jwt:Issuer", "LibraryAPI" },
                    { "Jwt:Audience", "LibraryUsers" }
                })

                .Build();

            _service = new AuthService(
                _context,
                configuration);
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

        private RegisterRequest CreateAdmin()
        {
            return new RegisterRequest
            {
                Username = "admin",

                Password = "Admin123",

                Name = "Administrator",

                Email = "admin@test.com"
            };
        }

        //----------------------------------------------------------
        // RegisterStudent Tests
        //----------------------------------------------------------

        [Test]
        public void RegisterStudent_ShouldReturnTrue_WhenUsernameIsUnique()
        {
            // Arrange

            var request = CreateStudent();

            // Act

            bool result = _service.RegisterStudent(request);

            // Assert

            Assert.That(result, Is.True);

            Assert.That(_context.Users.Count(), Is.EqualTo(1));

            Assert.That(_context.Members.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RegisterStudent_ShouldCreateStudentRole()
        {
            // Arrange

            var request = CreateStudent();

            // Act

            _service.RegisterStudent(request);

            // Assert

            var user = _context.Users.First();

            Assert.That(user.Role, Is.EqualTo("Student"));
        }

        [Test]
        public void RegisterStudent_ShouldHashPassword()
        {
            // Arrange

            var request = CreateStudent();

            // Act

            _service.RegisterStudent(request);

            // Assert

            var user = _context.Users.First();

            Assert.That(user.Password, Is.Not.EqualTo(request.Password));

            Assert.That(
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.Password),
                Is.True);
        }

        [Test]
        public void RegisterStudent_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            // Arrange

            var request = CreateStudent();

            _service.RegisterStudent(request);

            // Act

            bool result = _service.RegisterStudent(request);

            // Assert

            Assert.That(result, Is.False);

            Assert.That(_context.Users.Count(), Is.EqualTo(1));
        }

        //----------------------------------------------------------
        // RegisterAdmin Tests
        //----------------------------------------------------------

        [Test]
        public void RegisterAdmin_ShouldReturnTrue_WhenUsernameIsUnique()
        {
            // Arrange

            var request = CreateAdmin();

            // Act

            bool result = _service.RegisterAdmin(request);

            // Assert

            Assert.That(result, Is.True);

            Assert.That(_context.Users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RegisterAdmin_ShouldCreateAdminRole()
        {
            // Arrange

            var request = CreateAdmin();

            // Act

            _service.RegisterAdmin(request);

            // Assert

            var user = _context.Users.First();

            Assert.That(user.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public void RegisterAdmin_ShouldNotCreateMember()
        {
            // Arrange

            var request = CreateAdmin();

            // Act

            _service.RegisterAdmin(request);

            // Assert

            Assert.That(_context.Members.Count(), Is.EqualTo(0));
        }

        [Test]
        public void RegisterAdmin_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            // Arrange

            var request = CreateAdmin();

            _service.RegisterAdmin(request);

            // Act

            bool result = _service.RegisterAdmin(request);

            // Assert

            Assert.That(result, Is.False);

            Assert.That(_context.Users.Count(), Is.EqualTo(1));
        }

        //----------------------------------------------------------
        // Login Tests
        //----------------------------------------------------------

        [Test]
        public void Login_ShouldReturnLoginResponse_WhenCredentialsAreValid()
        {
            // Arrange

            var request = CreateStudent();

            _service.RegisterStudent(request);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Username, Is.EqualTo(request.Username));

            Assert.That(result.Role, Is.EqualTo("Student"));

            Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Login_ShouldReturnNull_WhenUsernameDoesNotExist()
        {
            // Arrange

            var loginRequest = new LoginRequest
            {
                Username = "UnknownUser",
                Password = "Password123"
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange

            var request = CreateStudent();

            _service.RegisterStudent(request);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = "WrongPassword"
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Login_ShouldGenerateJwtToken()
        {
            // Arrange

            var request = CreateStudent();

            _service.RegisterStudent(request);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Token.Length, Is.GreaterThan(100));
        }

        [Test]
        public void Login_ShouldReturnCorrectRole()
        {
            // Arrange

            var request = CreateAdmin();

            _service.RegisterAdmin(request);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public void Login_ShouldReturnCorrectUsername()
        {
            // Arrange

            var request = CreateStudent();

            _service.RegisterStudent(request);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            // Act

            var result = _service.Login(loginRequest);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Username, Is.EqualTo(request.Username));
        }
    }
}