using HotelsManagementSystem.Api.DTOs.Contact;
using HotelsManagementSystem.Api.Services.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelsManagementSystem.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IContactService _contactService;

        public ContactController(
            ILogger<ContactController> logger,
            IConfiguration configuration,
            IContactService contactService)
        {
            _logger = logger;
            _configuration = configuration;
            _contactService = contactService;
        }

        [HttpPost("send-message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactDTO inputDto)
        {
            var toEmail = _configuration.GetValue<string>("ContactForm:ToEmail");
            var senderEmail = _configuration.GetValue<string>("ContactForm:SenderEmail");

            try
            {
                await _contactService.SendContactFormEmailAsync(inputDto, toEmail, senderEmail);
                return Ok(new { message = "Contact message sent successfully." });
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error sending contact message.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while sending the contact message." });
            }
        }
    }
}
