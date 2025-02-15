using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvidersPlatform.Shared.Setup.Configs;
using ProvidersPlatform.Shared.Setup.Contexts;

namespace ProvidersPlatform.Shared.Setup.Defaults;

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

    public static void AddProvidersPlatformDatabase(this IServiceCollection service, MariaDbConfig config)
    {
        service.AddDbContext<ProvidersPlatformContext>(opt =>
        {
            opt.UseMySql(config.GetConnectionString(), new MariaDbServerVersion(config.ServerVersion));
        });
    }
}