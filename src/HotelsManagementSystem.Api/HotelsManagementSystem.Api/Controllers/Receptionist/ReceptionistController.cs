using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Services.Receptionist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Receptionist
{
    [Authorize(Roles = UserRoles.Receptionist)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly ILogger<ReceptionistController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReceptionistService _receptionistService;

        public ReceptionistController(
            ILogger<ReceptionistController> logger,
            UserManager<ApplicationUser> userManager,
            IReceptionistService receptionistService)
        {
            _logger = logger;
            _userManager = userManager;
            _receptionistService = receptionistService;
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReceptionistDashboard()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isUserReceptionist = await _userManager.IsInRoleAsync(user, UserRoles.Receptionist);
            if (!isUserReceptionist)
            {
                return Forbid();
            }

            var receptionistId = Guid.Parse(userId);
            var dashboardInfo = await _receptionistService.GetReceptionistDashBoardInfoAsync(receptionistId);

            return Ok(dashboardInfo);
        }
    }
}
