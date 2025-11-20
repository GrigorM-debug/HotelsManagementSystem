using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Auth;
using HotelsManagementSystem.Api.Services.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers.Admin.Receptionists
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminReceptionistController : ControllerBase
    {
        private readonly ILogger<AdminReceptionistController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHotelService _hotelService;

        public AdminReceptionistController(
            ILogger<AdminReceptionistController> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, 
            IHotelService hotelService)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _hotelService = hotelService;
        }

        [HttpPost("{hotelId}/create-receptionist")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReceptionist([FromBody] RegisterDTO inputDto, Guid hotelId)
        {
            var hotelExists = await _hotelService.HotelExistsByIdAsync(hotelId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel does not exist" });
            }

            var userExisting = await _userManager.FindByNameAsync(inputDto.UserName);
            if(userExisting != null)
            {
                return BadRequest(new { error = "User already exists" });
            }

            var newUser = new ApplicationUser
            {
                UserName = inputDto.UserName,
                Email = inputDto.Email,
                PhoneNumber = inputDto.PhoneNumber,
                FirstName = inputDto.FirstName,
                LastName = inputDto.LastName,
                JoinedOn = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, inputDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new {error = "Failed to create user" });
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Receptionist);

                var receptionist = new HotelsManagementSystem.Api.Data.Models.Users.Receptionist()
                {
                    HotelId = hotelId,
                    UserId = newUser.Id
                };

                await _context.Receptionists.AddAsync(receptionist);
                await _context.SaveChangesAsync();

                return Ok(new {success = "Receptionist created successfully" });
            }
        }
    }
}
