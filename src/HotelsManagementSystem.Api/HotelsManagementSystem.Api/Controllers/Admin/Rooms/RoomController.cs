using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.Services.Admin.Hotels;
using HotelsManagementSystem.Api.Services.Admin.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Admin.Rooms
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHotelService _hotelService;

        public RoomController(
            ILogger<RoomController> logger,
            IRoomService roomService,
            UserManager<ApplicationUser> userManager,
            IHotelService hotelService)
        {
            _logger = logger;
            _roomService = roomService;
            _userManager = userManager;
            _hotelService = hotelService;
        }

        [HttpGet("hotel/{hotelId}/rooms/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateRoom(Guid hotelId)
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

            var adminId = Guid.Parse(userId);
            var hotel = await _hotelService.GetHotelByIdAndAdminIdAsync(hotelId, adminId);
            if (hotel == null)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var createRoomGetDto = await _roomService.CreateRoomGetAsync();

            return Ok(createRoomGetDto);
        }

        [HttpPost("hotel/{hotelId}/rooms/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateRoom(Guid hotelId, [FromForm] CreateRoomPostDto inputDto)
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
            var hotelExists = await _hotelService.HotelExistsByHotelIdAndAdminIdAsync(hotelId, adminId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var roomExists = await _roomService.RoomExistsByRoomNumberAndHotelId(inputDto.RoomNumber, hotelId, adminId);
            if (roomExists)
            {
                return Conflict(new { error = "Room with the same room number already exists in this hotel." });
            }

            var selectedRoomTypeExists = await _roomService.RoomTypeExists(inputDto.RoomTypeId);
            if (!selectedRoomTypeExists)
            {
                return BadRequest(new { error = "Selected room type does not exist." });
            }

            var selectedFeaturesExist = await _roomService.FeaturesExistAsync(inputDto.FeatureIds);
            if (!selectedFeaturesExist)
            {
                return BadRequest(new { error = "One or more selected features do not exist." });
            }

            try
            {
                var newRoomId = await _roomService.CreateRoomPostAsync(inputDto, hotelId, adminId);

                return CreatedAtAction(nameof(CreateRoom), new {roomId = newRoomId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new room.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while creating the room. Please try again later." });
            }
        }

        [HttpGet("hotel/{hotelId}/rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetRoomsForHotel(Guid hotelId)
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
            var hotelExists = await _hotelService.HotelExistsByHotelIdAndAdminIdAsync(hotelId, adminId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var rooms = await _roomService.GetRoomsForHotelAsync(hotelId, adminId);

            return Ok(rooms);
        }
    }
}
