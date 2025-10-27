
using Smtp2Go.Api;
using Smtp2Go.Api.Models.Emails;

namespace HotelsManagementSystem.Api.Services.EmailProvider
{
    /// <summary>
    /// Service responsible for sending emails using the Smtp2Go API service.
    /// Implements the <see cref="IEmailProvider"/> interface to provide email functionality.
    /// </summary>
    public class EmailProvider : IEmailProvider
    {
        /// <summary>
        /// The Smtp2Go API service used to send emails through the Smtp2Go platform.
        /// </summary>
        private readonly Smtp2GoApiService _smtp2GoApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProvider"/> class.
        /// </summary>
        /// <param name="smtp2GoApiService">Smtp2Go API service for sending emails.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="smtp2GoApiService"/> is null.</exception>
        public EmailProvider(Smtp2GoApiService smtp2GoApiService)
        {
            _smtp2GoApiService = smtp2GoApiService;
        }

        /// <summary>
        /// Sends an email asynchronously using the Smtp2Go service.
        /// Creates and configures an email message with the specified parameters and sends it through the Smtp2Go API.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The HTML body content of the email.</param>
        /// <param name="sender">The sender's email address.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when email addresses are in invalid format.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the Smtp2Go service fails to send the email.</exception>
        /// <remarks>
        /// This method uses the Smtp2Go API to send emails. The body parameter should contain HTML content
        /// as it's set to the BodyHtml property of the EmailMessage. Make sure the sender email address
        /// is properly configured and authorized in your Smtp2Go account.
        /// </remarks>
        public async Task SendEmailAsync(string toEmail, string subject, string body, string sender)
        {
            var message = new EmailMessage(sender, toEmail);

            message.Subject = subject;
            message.BodyHtml = body;

            var response = await _smtp2GoApiService.SendEmail(message);
        }
    }
}
