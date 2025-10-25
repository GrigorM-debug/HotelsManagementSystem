using HotelsManagementSystem.Api.DTOs.Contact;

namespace HotelsManagementSystem.Api.Services.Contact
{
    public interface IContactService
    {
        public Task SendContactFormEmailAsync(ContactDTO contactData, string toEmail, string senderEmail);

    }
}
