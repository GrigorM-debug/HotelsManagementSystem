using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.Services.Admin.Hotels;
using HotelsManagementSystem.Api.Services.Admin.Hotels.Amentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Admin.Hotels
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelService _hotelService;
        private readonly IAmenityService _amenityService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HotelController(
            ILogger<HotelController> logger, 
            IHotelService hotelService,
            IAmenityService amenityService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _hotelService = hotelService;
            _amenityService = amenityService;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateHotel([FromForm] CreateHotelDto inputDto)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isUserAdmin)
            {
                return Forbid();
            }

            var hotelExists = await _hotelService.HotelExistsByNameAsync(inputDto.Name);
            if (hotelExists)
            {
                return Conflict(new { error = "A hotel with the same name already exists." });
            }

            var amenitiesExist = await _amenityService.AmenitiesExistsByIdsAsync(inputDto.AmenityIds);
            if (!amenitiesExist)
            {
                return BadRequest(new { error = "One or more selected amenities are invalid." });
            }

            var userIdToGuid = Guid.Parse(userId);

            var newHotelId = Guid.Empty;
            try
            {
                newHotelId = await _hotelService.CreateHotelAsync(inputDto, userIdToGuid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new hotel.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while creating the hotel. Please try again later." });
            }

            return CreatedAtAction(nameof(CreateHotel), new { hotelId = newHotelId });
        }
    }
}
