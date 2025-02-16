using MailKit.Security;

namespace ProvidersPlatform.EmailServices.Services.Custom;

public class SmtpServerConfig
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? UserHost { get; set; }
    public string? Alias { get; set; }
    public string? Key { get; set; }
    public SecureSocketOptions SecureSocketOptions { get; set; }
}