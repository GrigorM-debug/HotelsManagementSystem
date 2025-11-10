using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels.Edit;
using HotelsManagementSystem.Api.DTOs.Hotels;
using HotelsManagementSystem.Api.Services.Admin.Hotels;
using HotelsManagementSystem.Api.Services.Admin.Hotels.Amentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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

        [HttpGet("admin-hotels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAdminHotels([FromQuery] HotelsFilterDto? filter)
        {
            var adminId = _userManager.GetUserId(User);
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return NotFound(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(admin, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var adminIdToGuid = Guid.Parse(adminId);

            var hotels = await _hotelService.GetAdminHotelsAsync(adminIdToGuid, filter);
         
            return Ok(hotels);
        }

        [HttpDelete("{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteHotel(Guid hotelId)
        {
            var adminId = _userManager.GetUserId(User);

            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return NotFound(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(admin, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var adminIdToGuid = Guid.Parse(adminId);

            //Check if the hotelId is valid Guid
            bool isHotelIdValidGuid = Guid.TryParse(hotelId.ToString(), out _);
            if (!isHotelIdValidGuid)
            {
                return BadRequest(new { error = "Invalid hotel ID format." });
            }

            var hotelExists = await _hotelService.HotelExistsByHotelIdAndAdminIdAsync(hotelId, adminIdToGuid);
            if(!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var isDeletable = await _hotelService.IsHotelDeletableAsync(hotelId);
            if (!isDeletable)
            {
                return BadRequest(new { error = "The hotel cannot be deleted due to existing dependencies." });
            }

            bool isDeleted = await _hotelService.DeleteHotelAsync(hotelId, adminIdToGuid);

            if (!isDeleted)
            {
                return BadRequest(new { error = "Failed to delete the hotel. Please try again later." });
            }

            return Ok(new {success = "Hotel successfully deleted."});
        }

        [HttpGet("edit/{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetHotelForEditById(Guid hotelId)
        {
            var adminId = _userManager.GetUserId(User);
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(admin, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var adminIdToGuid = Guid.Parse(adminId);
            var hotelForEdit = await _hotelService.GetHotelForEditByIdAsync(hotelId, adminIdToGuid);
            if (hotelForEdit == null)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            return Ok(hotelForEdit);
        }

        [HttpPut("edit/{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditHotelPost([FromForm] EditHotelPostDto inputDto, Guid hotelId)
        {
            var adminId = _userManager.GetUserId(User);
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(admin, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var adminIdToGuid = Guid.Parse(adminId);
            var exitingHotel = await _hotelService.GetHotelByIdAndAdminIdAsync(hotelId, adminIdToGuid);

            if (exitingHotel == null)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            // Check if amenities exist
            var amenitiesExist = await _amenityService.AmenitiesExistsByIdsAsync(inputDto.AmenityIds);
            if (!amenitiesExist)
            {
                return BadRequest(new { error = "One or more selected amenities are invalid." });
            }

            // If the name is changed, check for uniqueness
            if (!string.IsNullOrEmpty(inputDto.Name) && inputDto.Name.ToLower() != exitingHotel.Name.ToLower())
            {
                var hotelWithSameNameExists = await _hotelService.HotelExistsByNameAsync(inputDto.Name);
                if (hotelWithSameNameExists)
                {
                    return Conflict(new { error = "A hotel with the same name already exists." });
                }
            }

            try
            {
                var isEdited = await _hotelService.EditHotelPostAsync(inputDto, adminIdToGuid, hotelId);

                if (!isEdited)
                {
                    return BadRequest(new { error = "Failed to edit the hotel. Please try again later." });
                }

                return Ok(new { success = "Hotel successfully edited." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the hotel.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while editing the hotel. Please try again later." });
            }
        }
    }
}
