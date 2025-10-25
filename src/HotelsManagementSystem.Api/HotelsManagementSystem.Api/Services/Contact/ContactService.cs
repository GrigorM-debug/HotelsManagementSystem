using HotelsManagementSystem.Api.DTOs.Contact;
using HotelsManagementSystem.Api.Services.EmailProvider;
using System.Text;

namespace HotelsManagementSystem.Api.Services.Contact
{
    /// <summary>
    /// Service responsible for handling contact form submissions and email notifications.
    /// Implements the <see cref="IContactService"/> interface to provide contact-related functionality.
    /// </summary>
    public class ContactService : IContactService
    {
        /// <summary>
        /// The email provider used to send contact form emails.
        /// </summary>
        private readonly IEmailProvider _emailProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactService"/> class.
        /// </summary>
        /// <param name="emailProvider">The email provider service used to send emails.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailProvider"/> is null.</exception>
        public ContactService(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
        }

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
        public async Task SendContactFormEmailAsync(ContactDTO contactData, string toEmail, string senderEmail)
        {
            var subject = $"New Contact Form Submission from {contactData.FirstName} {contactData.LastName}";

            var body = BuildEmailHtmlBody(contactData);

            await _emailProvider.SendEmailAsync(toEmail, subject, body, senderEmail);
        }

        /// <summary>
        /// Builds a formatted HTML email body containing the contact form data.
        /// Creates a professional-looking HTML email template with styling and contact information.
        /// </summary>
        /// <param name="contactData">The contact form data to be formatted into HTML.</param>
        /// <returns>A formatted HTML string containing the contact form information with styling and structure.</returns>
        /// <remarks>
        /// The generated HTML includes:
        /// - Professional styling with CSS
        /// - Contact person's full name, email, and phone number
        /// - The submitted message with proper formatting
        /// - Timestamp of submission
        /// - Hotel management system branding
        /// </remarks>
        private string BuildEmailHtmlBody(ContactDTO contactData)
        {
            var htmlBody = new StringBuilder();

            htmlBody.AppendLine("<!DOCTYPE html>");
            htmlBody.AppendLine("<html>");
            htmlBody.AppendLine("<head>");
            htmlBody.AppendLine("    <meta charset='utf-8'>");
            htmlBody.AppendLine("    <title>Contact Form Submission</title>");
            htmlBody.AppendLine("    <style>");
            htmlBody.AppendLine("        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
            htmlBody.AppendLine("        .container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            htmlBody.AppendLine("        .header { background-color: #2c3e50; color: white; padding: 20px; text-align: center; }");
            htmlBody.AppendLine("        .content { background-color: #f9f9f9; padding: 20px; }");
            htmlBody.AppendLine("        .field { margin-bottom: 15px; }");
            htmlBody.AppendLine("        .label { font-weight: bold; color: #2c3e50; }");
            htmlBody.AppendLine("        .value { margin-top: 5px; padding: 10px; background-color: white; border-left: 3px solid #3498db; }");
            htmlBody.AppendLine("        .message-box { background-color: white; padding: 15px; border: 1px solid #ddd; border-radius: 5px; }");
            htmlBody.AppendLine("        .footer { text-align: center; padding: 20px; font-size: 12px; color: #7f8c8d; }");
            htmlBody.AppendLine("    </style>");
            htmlBody.AppendLine("</head>");
            htmlBody.AppendLine("<body>");

            htmlBody.AppendLine("<div class='container'>");
            htmlBody.AppendLine("    <div class='header'>");
            htmlBody.AppendLine("        <h1>🏨 Hotels Management System</h1>");
            htmlBody.AppendLine("        <h2>New Contact Form Submission</h2>");
            htmlBody.AppendLine("    </div>");

            htmlBody.AppendLine("    <div class='content'>");
            htmlBody.AppendLine("        <p>You have received a new message through the contact form:</p>");

            htmlBody.AppendLine("        <div class='field'>");
            htmlBody.AppendLine("            <div class='label'>👤 Full Name:</div>");
            htmlBody.AppendLine($"            <div class='value'>{contactData.FirstName} {contactData.LastName}</div>");
            htmlBody.AppendLine("        </div>");

            htmlBody.AppendLine("        <div class='field'>");
            htmlBody.AppendLine("            <div class='label'>📧 Email Address:</div>");
            htmlBody.AppendLine($"            <div class='value'><a href='mailto:{contactData.Email}'>{contactData.Email}</a></div>");
            htmlBody.AppendLine("        </div>");

            htmlBody.AppendLine("        <div class='field'>");
            htmlBody.AppendLine("            <div class='label'>📞 Phone Number:</div>");
            htmlBody.AppendLine($"            <div class='value'><a href='tel:{contactData.PhoneNumber}'>{contactData.PhoneNumber}</a></div>");
            htmlBody.AppendLine("        </div>");

            htmlBody.AppendLine("        <div class='field'>");
            htmlBody.AppendLine("            <div class='label'>💬 Message:</div>");
            htmlBody.AppendLine("            <div class='message-box'>");
            htmlBody.AppendLine($"                {contactData.Message.Replace("\n", "<br>").Replace("\r", "")}");
            htmlBody.AppendLine("            </div>");
            htmlBody.AppendLine("        </div>");

            htmlBody.AppendLine("        <div class='field'>");
            htmlBody.AppendLine("            <div class='label'>📅 Submitted On:</div>");
            htmlBody.AppendLine($"            <div class='value'>{DateTime.Now:dddd, MMMM dd, yyyy 'at' HH:mm}</div>");
            htmlBody.AppendLine("        </div>");
            htmlBody.AppendLine("    </div>");

            htmlBody.AppendLine("    <div class='footer'>");
            htmlBody.AppendLine("        <p>This email was automatically generated from your hotel website's contact form.</p>");
            htmlBody.AppendLine("        <p>Please respond to the customer directly using the provided email address.</p>");
            htmlBody.AppendLine("    </div>");
            htmlBody.AppendLine("</div>");

            htmlBody.AppendLine("</body>");
            htmlBody.AppendLine("</html>");

            return htmlBody.ToString();
        }
    }
}
