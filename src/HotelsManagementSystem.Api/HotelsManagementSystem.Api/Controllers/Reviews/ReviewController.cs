using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Reviews;
using HotelsManagementSystem.Api.Services.Hotels;
using HotelsManagementSystem.Api.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Reviews
{
    [Authorize(Roles = UserRoles.Customer)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHotelService _hotelService;

        public ReviewController(
            ILogger<ReviewController> logger, 
            IReviewService reviewService,
            UserManager<ApplicationUser> userManager,
            IHotelService hotelService)
        {
            _logger = logger;
            _reviewService = reviewService;
            _userManager = userManager;
            _hotelService = hotelService;
        }

        [HttpPost("hotels/{hotelId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateReview(Guid hotelId, [FromBody] CreateReviewDto inputDto)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isUserCustomer = await _userManager.IsInRoleAsync(user, UserRoles.Customer);
            if (!isUserCustomer)
            {
                return Forbid();
            }

            var customerId = Guid.Parse(userId);

            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if(!hotelExists)
            {
                return NotFound(new {error = "Hotel doesn't exist."});
            }

            var reviewAlreadyExists = await _reviewService.ReviewAlreadyExistsByCustomerIdAndHotelIdAsync(customerId, hotelId);
            if (reviewAlreadyExists)
            {
                return BadRequest(new { error = "Review already added." });
            }

            var isCreated = await _reviewService.CreateReviewAsync(customerId, hotelId, inputDto);

            if (isCreated)
            {
                return Ok(new {success = "Review successfully created."});
            }
            else
            {
                return BadRequest(new { error = "Something happen while creating the review." });
            }
        }
    }
}
