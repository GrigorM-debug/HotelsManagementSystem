using HotelsManagementSystem.Api.Services.Hotels;
using HotelsManagementSystem.Api.Services.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;

        public RoomController(
            ILogger<RoomController> logger,
            IHotelService hotelService,
            IRoomService roomService)
        {
            _logger = logger;
            _hotelService = hotelService;
            _roomService = roomService;
        }

        [HttpGet("hotel/{hotelId}/rooms/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetRoomDetails(Guid hotelId, Guid roomId)
        {
            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var roomExists = await _roomService.RoomExistsByIdAndHotelIdAsync(roomId, hotelId);
            if (!roomExists)
            {
                return NotFound(new { error = "Room not found." });
            }

            var room = await _roomService.GetRoomByIdAndHotelIdAsync(roomId, hotelId);

            return Ok(room);
        }
    }
}
