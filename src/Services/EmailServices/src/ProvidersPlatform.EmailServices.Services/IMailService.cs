using MimeKit;
using ProvidersPlatform.EmailServices.Services.Custom;

namespace ProvidersPlatform.EmailServices.Services;

public interface IMailService
{
    public Task<bool> SendMailAsync(string subject, string body, MailboxAddress to, SmtpServerConfig config);
}