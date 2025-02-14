using ProvidersPlatform.Shared.Setup;

namespace ProvidersPlatform.UserServices.Rest;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            opt.Services.AddControllers();
        });
        
        app.MapControllers();
        app.MapGet("/healthy", ()=> "Rest Api User Healthy");
        DefaultApiProject.RunApplication(app);
    }
}