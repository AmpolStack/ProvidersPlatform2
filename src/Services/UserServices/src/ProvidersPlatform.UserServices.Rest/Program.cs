using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.EmailServices.Grpc;
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
            
            //Inject Grpc Clients
            opt.Services.AddGrpcClient<MailOperations.MailOperationsClient>(conf =>
            {
                var https = opt.Configuration["GrpcServer:https"];
                conf.Address = new Uri(https!);
            }).ConfigureChannel(conf =>
            {
                //Configuration to Service Config
                conf.ServiceConfig = new ServiceConfig()
                {
                    //Method configs
                    MethodConfigs =
                    {
                        new MethodConfig()
                        {
                            Names = { MethodName.Default },
                            RetryPolicy = new RetryPolicy()
                            {
                                MaxAttempts = 3,
                                BackoffMultiplier = 1.2,
                                InitialBackoff = TimeSpan.FromSeconds(1),
                                MaxBackoff = TimeSpan.FromSeconds(5),
                                RetryableStatusCodes = { StatusCode.Unavailable }
                            }
                        }
                    }
                };
            });
            
        });
        
        app.MapControllers();
        app.MapGet("/healthy", ()=> "Rest Api User Healthy");
        
        DefaultApiProject.RunApplication(app);
    }
}