using HotelsManagementSystem.Api.DTOs.Hotels;
using HotelsManagementSystem.Api.Services.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelService _hotelService;
        public HotelController(
            ILogger<HotelController> logger,
            IHotelService hotelService)
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        [HttpGet("{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHotelDetails(Guid hotelId)
        {
            var isHotelIdValidGuid = Guid.TryParse(hotelId.ToString(), out var _);
            if (!isHotelIdValidGuid)
            {
                return NotFound(new { error = "Invalid hotel id" });
            }

            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel not found." });
            }

            var hotelDetails = await _hotelService.GetHotelDetailsByIdAsync(hotelId);

            return Ok(hotelDetails);
        }

        [HttpGet("hotels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetHotels([FromQuery] HotelsFilterDto? filter)
        {
            var hotels = await _hotelService.GetHotelsAsync(filter);


            return Ok(new {fetchedHotels = hotels});
        }
    }
}
