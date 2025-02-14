using ProvidersPlatform.Shared.Setup;

namespace ProvidersPlatform.PostServices.GraphQl;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate();
        
        app.MapGet("/healthy", () => "GraphQl Posts Api Are Healthy");
        
        DefaultApiProject.RunApplication(app);
    }
}