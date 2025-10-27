namespace HotelsManagementSystem.Api.Services.EmailProvider
{
    /// <summary>
    /// Service responsible for sending emails using the Smtp2Go API service.
    /// Implements the <see cref="IEmailProvider"/> interface to provide email functionality.
    /// </summary>
    public interface IEmailProvider
    {
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
        public Task SendEmailAsync(string toEmail, string subject, string body, string sender);
    }
}
