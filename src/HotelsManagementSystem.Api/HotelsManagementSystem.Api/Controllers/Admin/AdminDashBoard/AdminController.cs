using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Services.Admin.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Admin.AdminDashBoard
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            ILogger<AdminController> logger,
            IAdminService adminService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _adminService = adminService;
            _userManager = userManager;
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isUserAdmin)
            {
                return Forbid();
            }

            var adminId = Guid.Parse(userId); 

            var dashboardInfo = await _adminService.GetAdminDashBoardInfoAsync(adminId);

            return Ok(dashboardInfo);
        }
    }
}
