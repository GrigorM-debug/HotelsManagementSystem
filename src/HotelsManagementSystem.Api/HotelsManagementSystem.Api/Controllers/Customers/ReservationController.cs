using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Customers.Reservations;
using HotelsManagementSystem.Api.Services.Customers.Reservation;
using HotelsManagementSystem.Api.Services.Hotels;
using HotelsManagementSystem.Api.Services.Rooms;
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
        private readonly IRoomService _roomService;

        public ReservationController(
            ILogger<ReservationController> logger,
            UserManager<ApplicationUser> userManager,
            IHotelService hotelService,
            IReservationService reservationService, 
            IRoomService roomService)
        {
            _logger = logger;
            _userManager = userManager;
            _hotelService = hotelService;
            _reservationService = reservationService;
            _roomService = roomService;
        }

        [HttpGet("hotel/{hotelId}/available-rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetHotelAvailableRooms(
            Guid hotelId, 
            [FromQuery] ReservationHotelRoomsFilter? filter)
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

            return Ok(avaibleRooms);
        }

        [HttpPost("hotel/{hotelId}/room/{roomId}/book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> BookRoom(
            Guid hotelId, 
            Guid roomId, 
            ReservationCreateDto reservationCreateDto)
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

            // Check if hotel exists
            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            // Check if room exists and belongs to the hotel
            var room = await _roomService.GetRoomByIdAndHotelIdAsync(roomId, hotelId);
            if (room == null)
            {
                return NotFound(new { error = "Room not found in the specified hotel." });
            }

            if(DateTime.TryParse(reservationCreateDto.CheckInDate, out var checkInDate) &&
               DateTime.TryParse(reservationCreateDto.CheckOutDate, out var checkOutDate))
            {
                if(checkInDate >= checkOutDate)
                {
                    return BadRequest(new { error = "Check-out date must be after check-in date." });
                }
            }
            else
            {
                return BadRequest(new { error = "Invalid date format." });
            }

            if(reservationCreateDto.NumberOfGuests <= 0)
            {
                return BadRequest(new { error = "Number of guests must be greater than zero." });
            }

            if(reservationCreateDto.NumberOfGuests > room.RoomType.Capacity)
            {
                return BadRequest(new { error = $"Number of guests exceeds room capacity of {room.RoomType.Capacity}." });
            }

            if(reservationCreateDto.NumberOfGuests < room.RoomType.Capacity)
            {
                return BadRequest(new { error = $"Number of guests is less than room capacity of {room.RoomType.Capacity}." });
            }

            // Check if reservation already exists for the specified room and dates
            var reservationExists = await _reservationService.ReservationAlreadyExists(
                hotelId, 
                roomId, 
                checkInDate, 
                checkOutDate);
            if (reservationExists)
            {
                return BadRequest(new { error = "A reservation already exists for the specified room and dates." });
            }

            var userIdToGuid = Guid.Parse(userId);

            var checkInDateToUtc = checkInDate.ToUniversalTime();
            var checkOutDateToUtc = checkOutDate.ToUniversalTime();

            var isBookedSuccessfully = await _reservationService.CreateRoomReservationsAsync(
                userIdToGuid, 
                hotelId, 
                roomId, 
                checkInDate, 
                checkOutDate,
                reservationCreateDto.NumberOfGuests);

            if (isBookedSuccessfully)
            {
                return Ok(new { success = "Room successfully booked." });
            }
            else
            {
                return BadRequest(new { error = "Failed to book the room." });
            }
        }

        [HttpGet("my-reservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCustomerReservations()
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

            var userIdToGuid = Guid.Parse(userId);

            var customerReservations = await _reservationService.GetCustomerReservationsAsync(userIdToGuid);

            return Ok(customerReservations);
        }

        [HttpPost("{reservationId}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CancelReservation(Guid reservationId)
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

            var userIdToGuid = Guid.Parse(userId);
            var reservationExists = await _reservationService.ReservationExistsByCustomerIdAndReservationIdAsync(reservationId, userIdToGuid);
            if (!reservationExists)
            {
                return NotFound(new { error = "Reservation not found." });
            }

            var isAlreadyCancelled = await _reservationService.CheckIfReservationIsAlreadyCancelledAsync(reservationId, userIdToGuid);
            if (isAlreadyCancelled)
            {
                return BadRequest(new { error = "Reservation is already cancelled." });
            }

            var isCancelledSuccessfully = await _reservationService.CancelReservationAsync(
                reservationId, 
                userIdToGuid);
            if (isCancelledSuccessfully)
            {
                return Ok(new { success = "Reservation successfully cancelled." });
            }
            else
            {
                return BadRequest(new { error = "Failed to cancel the reservation." });
            }
        }
    }
}
