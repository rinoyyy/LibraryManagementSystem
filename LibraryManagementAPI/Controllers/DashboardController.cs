using LibraryManagementAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IUserService _userService;

        public DashboardController(
            IDashboardService dashboardService,
            IUserService userService)
        {
            _dashboardService = dashboardService;
            _userService = userService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("student")]
        public IActionResult GetStudentDashboard()
        {
            var username = User.Identity!.Name!;

            var memberId = _userService.GetMemberId(username);

            if (memberId == null)
            {
                return Unauthorized();
            }

            return Ok(
                _dashboardService.GetStudentDashboard(memberId.Value)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(
                _dashboardService.GetAdminDashboard()
            );
        }
    }
}