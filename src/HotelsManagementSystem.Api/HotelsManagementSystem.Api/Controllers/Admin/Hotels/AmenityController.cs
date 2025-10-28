using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Services.Admin.Hotels.Amentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Admin.Hotels
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly ILogger<AmenityController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAmenityService _amenityService;

        public AmenityController(
            ILogger<AmenityController> logger,
            UserManager<ApplicationUser> userManager,
            IAmenityService amenityService)
        {
            _logger = logger;
            _userManager = userManager;
            _amenityService = amenityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAmentities()
        {
            var userId = _userManager.GetUserId(User);

            // Check if user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            // Check if user is admin
            var isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var amenities = await _amenityService.GetAllAmentitiesAsync();

            return Ok(amenities);
        }
    }
}
