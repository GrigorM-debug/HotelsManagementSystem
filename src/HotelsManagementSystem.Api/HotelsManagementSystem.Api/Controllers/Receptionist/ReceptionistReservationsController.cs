using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.Services.Receptionist.ReceptionistReservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Receptionist
{
    [Authorize(Roles = UserRoles.Receptionist)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReceptionistReservationsController : ControllerBase
    {
        private readonly ILogger<ReceptionistReservationsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReceptionistReservationsService _receptionistReservationsService;
        public ReceptionistReservationsController(
            ILogger<ReceptionistReservationsController> logger,
            UserManager<ApplicationUser> userManager,
            IReceptionistReservationsService receptionistReservationsService)
        {
            _logger = logger;
            _userManager = userManager;
            _receptionistReservationsService = receptionistReservationsService;
        }

        [HttpGet("reservations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReservations()
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
            var reservations = await _receptionistReservationsService.GetReservationsAsync(receptionistId);

            return Ok(reservations);
        }

        [HttpPost("confirm-reservation/{reservationId}/customer/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> ConfirmReservation(Guid reservationId, Guid customerId)
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

            var reservation = await _receptionistReservationsService.GetReservationAsync(reservationId, customerId);
            if (reservation == null)
            {
                return NotFound(new { error = "Reservation not found." });
            }

            if(reservation.ReservationStatus != Enums.ReservationStatus.Pending)
            {
                return BadRequest(new { error = "Only pending reservations can be confirmed." });
            }

            var receptionistId = Guid.Parse(userId);    

            var isConfirmed = await _receptionistReservationsService.ConfirmReservationAsync(reservationId, customerId, receptionistId);

            if (!isConfirmed)
            {
                return BadRequest(new { error = "Failed to confirm reservation." });
            }

            return Ok(new { success = "Reservation confirmed successfully." });
        }

        [HttpPost("check-in-reservation/{reservationId}/customer/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CheckInReservation(Guid reservationId, Guid customerId)
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

            var reservation = await _receptionistReservationsService.GetReservationAsync(reservationId, customerId);
            if (reservation == null)
            {
                return NotFound(new { error = "Reservation not found." });
            }

            if (reservation.ReservationStatus != Enums.ReservationStatus.Confirmed)
            {
                return BadRequest(new { error = "Only confirmed reservations can be checked in." });
            }

            var receptionistId = Guid.Parse(userId);
            var isCheckedIn = await _receptionistReservationsService.CheckInReservationAsync(reservationId, customerId, receptionistId);

            if (!isCheckedIn)
            {
                return BadRequest(new { error = "Failed to check in reservation." });
            }

            return Ok(new { success = "Reservation checked in successfully." });
        }
    }
}
