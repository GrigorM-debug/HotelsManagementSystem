using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Auth;
using HotelsManagementSystem.Api.Services.Admin.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateReceptionist([FromBody] RegisterDTO inputDto, Guid hotelId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }
            
            var adminId = Guid.Parse(userId);
            var hotelExists = await _hotelService.HotelExistsByHotelIdAndAdminIdAsync(hotelId, adminId);
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

        [HttpDelete("{hotelId}/delete-receptionist/{receptionistId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteReceptionist(Guid hotelId, Guid receptionistId)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin)
            {
                return Forbid();
            }

            var adminId = Guid.Parse(userId);
            var hotelExists = await _hotelService.HotelExistsByHotelIdAndAdminIdAsync(hotelId, adminId);
            if (!hotelExists)
            {
                return NotFound(new { error = "Hotel does not exist" });
            }

            var receptionist = await _userManager.FindByIdAsync(receptionistId.ToString());
            var receptionistProfile = await _context.Receptionists
                .FirstOrDefaultAsync(r =>
                    r.HotelId == hotelId &&
                    r.UserId == receptionistId);
            if (receptionist == null && receptionistProfile == null)
            {
                return NotFound(new { error = "Receptionist not found" });
            }

            // delete receptionist 
            var result = await _userManager.DeleteAsync(receptionist);

            if (result.Succeeded)
            {
                _context.Receptionists.Remove(receptionistProfile);
                await _context.SaveChangesAsync();

                return Ok(new { success = "Receptionist deleted successfully" });
            }
            else
            {
                return BadRequest(new { error = "Failed to delete receptionist" });
            }
        }
    }
}
