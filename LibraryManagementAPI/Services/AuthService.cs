using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context,
                   IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool RegisterStudent(RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return false;
            }

            var user = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Student"
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            var member = new Member
            {
                UserId = user.Id,
                Name = request.Name,
                Email = request.Email
            };

            _context.Members.Add(member);

            _context.SaveChanges();

            return true;
        }

        public bool RegisterAdmin(RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return false;
            }

            var user = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Admin"
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            return true;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                return null;
            }

            bool passwordMatches =
                BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!passwordMatches)
            {
                return null;
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}