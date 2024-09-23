using Common.Dtos;

namespace Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageDto message);
        Task SendHtmlEmailAsync(EmailMessageDto message);
    }
}