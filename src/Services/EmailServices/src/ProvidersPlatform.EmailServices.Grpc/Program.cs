using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using ProvidersPlatform.EmailServices.Grpc.Services;
using ProvidersPlatform.EmailServices.Services;
using ProvidersPlatform.EmailServices.Services.Custom;

namespace ProvidersPlatform.EmailServices.Grpc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.Configure<SmtpServerConfig>(builder.Configuration.GetSection("SmtpServer"));
        builder.Services.AddScoped<IMailService, MailService>();

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        app.MapGrpcService<MailOperationsServices>();
        app.MapGet("/sm", async ([FromServices] IMailService mail, [FromServices] IOptions<SmtpServerConfig> config) =>
        {
            var todo = new Todo()
            {
                Subject = "hi, this is a test",
                Body = "Hi,this is a test mail 1"
            };
            
            var temp = config.Value;
            var operation = await mail.SendMailAsync(todo.Subject,
                todo.Body,
                new MailboxAddress("smapsi", "sacount571@gmail.com"),
                config.Value);
            return Results.Ok(operation + " según todo bien");
        });
        
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}

public record Todo
{

    public string? Subject { get; set; }
    public string? Body { get; set; }
}