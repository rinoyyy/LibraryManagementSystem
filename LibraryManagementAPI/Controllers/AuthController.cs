using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/student")]
        public IActionResult RegisterStudent(RegisterRequest request)
        {
            var success = _authService.RegisterStudent(request);

            if (!success)
            {
                return BadRequest("Username already exists.");
            }

            return Ok("Student registered successfully.");
        }

        [HttpPost("register/admin")]
        public IActionResult RegisterAdmin(RegisterRequest request)
        {
            var success = _authService.RegisterAdmin(request);

            if (!success)
            {
                return BadRequest("Username already exists.");
            }

            return Ok("Admin registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = _authService.Login(request);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}