using Photoforge_Server.Models;

namespace Photoforge_Server.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest email);
    }
}
