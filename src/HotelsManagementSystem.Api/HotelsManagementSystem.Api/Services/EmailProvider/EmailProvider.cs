
using Smtp2Go.Api;
using Smtp2Go.Api.Models.Emails;

namespace HotelsManagementSystem.Api.Services.EmailProvider
{
    public class EmailProvider : IEmailProvider
    {
        private readonly Smtp2GoApiService _smtp2GoApiService;

        public EmailProvider(Smtp2GoApiService smtp2GoApiService)
        {
            _smtp2GoApiService = smtp2GoApiService;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string sender)
        {
            var message = new EmailMessage(sender, toEmail);

            message.Subject = subject;
            message.BodyHtml = body;

            var response = await _smtp2GoApiService.SendEmail(message);
        }
    }
}
