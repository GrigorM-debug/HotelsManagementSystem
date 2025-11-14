using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Customers.Reservation;
using HotelsManagementSystem.Api.Services.Customers.Reservation;
using HotelsManagementSystem.Api.Services.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Customers
{
    [Authorize(Roles = UserRoles.Customer)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHotelService _hotelService;
        private readonly IReservationService _reservationService;

        public ReservationController(
            ILogger<ReservationController> logger,
            UserManager<ApplicationUser> userManager,
            IHotelService hotelService,
            IReservationService reservationService)
        {
            _logger = logger;
            _userManager = userManager;
            _hotelService = hotelService;
            _reservationService = reservationService;
        }

        [HttpGet("hotel/{hotelId}/available-rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetHotelAvailableRooms(Guid hotelId, [FromQuery] ReservationHotelRoomsFilter? filter)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isCustomer = await _userManager.IsInRoleAsync(user, UserRoles.Customer);
            if (!isCustomer)
            {
                return Forbid();
            }

            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var avaibleRooms = await _reservationService.GetHotelAvailableRoomsAsync(hotelId, filter);

            return Ok(new { availableRooms = avaibleRooms });
        }
    }
}
