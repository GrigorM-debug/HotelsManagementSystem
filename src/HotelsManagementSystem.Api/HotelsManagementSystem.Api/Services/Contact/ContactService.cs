using HotelsManagementSystem.Api.DTOs.Contact;
using HotelsManagementSystem.Api.Services.EmailProvider;
using System.Text;

namespace HotelsManagementSystem.Api.Services.Contact
{
    public class ContactService : IContactService
    {
        private readonly IEmailProvider _emailProvider;

        public ContactService(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
        }

        public async Task SendContactFormEmailAsync(ContactDTO contactData, string toEmail, string senderEmail)
        {
            var subject = $"New Contact Form Submission from {contactData.FirstName} {contactData.LastName}";

            var body = BuildEmailHtmlBody(contactData);

            await _emailProvider.SendEmailAsync(toEmail, subject, body, senderEmail);
        }

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
            htmlBody.AppendLine("        <h1>🏨 Hotel Management System</h1>");
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
