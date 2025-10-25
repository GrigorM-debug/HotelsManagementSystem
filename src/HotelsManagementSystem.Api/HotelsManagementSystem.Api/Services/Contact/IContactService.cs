using HotelsManagementSystem.Api.DTOs.Contact;

namespace HotelsManagementSystem.Api.Services.Contact
{
    /// <summary>
    /// Service responsible for handling contact form submissions and email notifications.
    /// Implements the <see cref="IContactService"/> interface to provide contact-related functionality.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Sends a contact form email asynchronously with the provided contact data.
        /// Creates a formatted HTML email containing the contact information and sends it to the specified recipient.
        /// </summary>
        /// <param name="contactData">The contact form data containing user information and message.</param>
        /// <param name="toEmail">The recipient email address where the contact form will be sent.</param>
        /// <param name="senderEmail">The sender email address used to send the contact form.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when email addresses are in invalid format.</exception>
        public Task SendContactFormEmailAsync(ContactDTO contactData, string toEmail, string senderEmail);
    }
}
