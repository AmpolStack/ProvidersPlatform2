using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ProvidersPlatform.Shared.Setup;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.Gateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            opt.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            opt.Services.AddOcelot();
            
        });


        app.UseOcelot().Wait();
        app.MapGet("/healthy", () => "Api Gateway Healthy");

        DefaultApiProject.RunApplication(app);
    }
}