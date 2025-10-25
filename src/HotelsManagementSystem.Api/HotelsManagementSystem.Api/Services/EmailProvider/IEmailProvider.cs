namespace HotelsManagementSystem.Api.Services.EmailProvider
{
    public interface IEmailProvider
    {
        public Task SendEmailAsync(string toEmail, string subject, string body, string sender);
    }
}
