using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Configs;
using ProvidersPlatform.Shared.Setup.Contexts;
using ProvidersPlatform.Shared.Setup.Defaults;
using ProvidersPlatform.UserServices.Rest.Utilities;
using ProvidersPlatform.UserServices.Services.Definitions;
using ProvidersPlatform.UserServices.Services.Implementations;

namespace ProvidersPlatform.UserServices.Rest;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            //Inject Regular Services
            opt.Services.AddControllers();
            
            //Inject Custom Services
            opt.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            opt.Services.AddScoped<IUserRepository, UserRepository>();
            opt.Services.AddAutoMapper(typeof(AutoMapperProfile));
            
            //Inject Databases
            var mariaDbConfig = new MariaDbConfig();
            opt.Configuration.GetSection("Databases:Providers_Platform").Bind(mariaDbConfig);
            opt.Services.AddProvidersPlatformDatabase(mariaDbConfig);
            
        });
        
        app.MapControllers();
        app.MapGet("/healthy", ()=> "Rest Api User Healthy");
        
        DefaultApiProject.RunApplication(app);
    }
}