using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProvidersPlatform.Shared.Setup;

public static class DefaultApiProject
{
    public static WebApplication GenerateWebApplicationTemplate(Action<WebApplicationBuilder>? customSettings = null)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        customSettings?.Invoke(builder);
        return builder.Build();
    }

    public static void RunApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.Run();
    }

}