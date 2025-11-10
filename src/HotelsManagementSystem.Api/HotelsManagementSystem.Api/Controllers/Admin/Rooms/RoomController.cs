using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
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
    }
}
