using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Auth;
using HotelsManagementSystem.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelsManagementSystem.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenProviderService _tokenProviderService;
        private readonly ApplicationDbContext _context;

        public AuthController(
            ILogger<AuthController> logger, 
            UserManager<ApplicationUser> userManager, 
            ITokenProviderService tokenProviderService,
            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenProviderService = tokenProviderService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Forbid();
            }       

            var user = await _userManager.FindByNameAsync(loginDTO.UserName);

            if (user is null)
            {
                return NotFound(new { error = "User not found"});
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (isPasswordValid == false)
            {
                return BadRequest(new {error = "Wrong password"});
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token 
            var token = _tokenProviderService.GenerateToken(user, roles);

            var response = new AuthResponse()
            {
                UserName = user.UserName,
                Email = user.Email,
                AccessToken = token,
                Roles = roles.ToList()
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register ([FromBody] RegisterDTO registerDTO)
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            // Check if user with the same username already exists
            var user = await _userManager.FindByNameAsync(registerDTO.UserName);

            if(user != null)
            {
                return BadRequest(new {error = "User already exists" });
            }

            var newUser = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                JoinedOn = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);

                var customerProfile = new Customer()
                {
                    UserId = newUser.Id,
                    User = newUser
                };

                await _context.Customers.AddAsync(customerProfile);
                await _context.SaveChangesAsync();
            }

            return Created();
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Logout()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound(new {error = "User not found"});
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            await _tokenProviderService.AddTokenToBlackList(token);

            return Ok(new { message = "Successfully logged out" });
        }   
    }
}
