using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Users;
using HotelsManagementSystem.Api.DTOs.Auth;
using HotelsManagementSystem.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers
{
    [AllowAnonymous]
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

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);

            if (user is null)
            {
                return NotFound("User not found");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (isPasswordValid == false)
            {
                return Unauthorized("Invalid password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token 
            var token = _tokenProviderService.GenerateToken(user, roles);

            var response = new AuthResponse()
            {
                UserName = user.UserName,
                Email = user.Email,
                AccessToken = token
            };

            return Ok(response);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register ([FromBody] RegisterDTO registerDTO)
        {
            // Check if user with the same username already exists
            var user = await _userManager.FindByNameAsync(registerDTO.UserName);

            if(user != null)
            {
                return BadRequest("User already exists");
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
    }
}
