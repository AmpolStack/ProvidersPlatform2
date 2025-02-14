using ProvidersPlatform.Shared.Setup;

namespace ProvidersPlatform.Gateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            opt.Services.AddControllers();
        });

        app.MapControllers();
        app.MapGet("/healthy", () => "Api Gateway Healthy");

        DefaultApiProject.RunApplication(app);
    }
}