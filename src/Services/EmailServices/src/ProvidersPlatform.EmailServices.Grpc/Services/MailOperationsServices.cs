using System.Diagnostics;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MimeKit;
using ProvidersPlatform.EmailServices.Services;
using ProvidersPlatform.EmailServices.Services.Custom;

namespace ProvidersPlatform.EmailServices.Grpc.Services;

public class MailOperationsServices : MailOperations.MailOperationsBase
{
    private readonly IMailService _mailService;
    private readonly SmtpServerConfig _smtpServerConfig;

    public MailOperationsServices(IMailService mailService, IOptions<SmtpServerConfig> smtpServerConfig)
    {
        _mailService = mailService;
        _smtpServerConfig = smtpServerConfig.Value;
    }

    public override async Task<MailReply> SendMail(MailRequest request, ServerCallContext context)
    {
        var response = new MailReply();

        try
        {
            var sb = new Stopwatch();
            sb.Start();
            var operation = await _mailService.SendMailAsync(request.Subject,
                request.Body,
                new MailboxAddress(request.AliasTo, request.To),
                _smtpServerConfig);

            context.ResponseTrailers.Add("ExecTime", sb.ElapsedMilliseconds.ToString());
            sb.Stop();
            response.Success = operation;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Error = ex.Message;
        }
        
        return response;
    }
}