using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ProvidersPlatform.EmailServices.Services.Custom;

namespace ProvidersPlatform.EmailServices.Services;

public class MailService : IMailService
{
    
    public async Task<bool> SendMailAsync(string subject, string body, MailboxAddress to, SmtpServerConfig config)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(config.Alias, config.UserHost));
        message.To.Add(to);
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(config.Host, config.Port, config.SecureSocketOptions);
            await client.AuthenticateAsync(config.UserHost, config.Key);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        return true;
    }

    
}