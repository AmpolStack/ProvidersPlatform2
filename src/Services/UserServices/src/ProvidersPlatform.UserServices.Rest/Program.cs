using System.Text;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProvidersPlatform.EmailServices.Grpc;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Configs;
using ProvidersPlatform.Shared.Setup.Contexts;
using ProvidersPlatform.Shared.Setup.Defaults;
using ProvidersPlatform.UserServices.Rest.Utilities;
using ProvidersPlatform.UserServices.Services.Custom;
using ProvidersPlatform.UserServices.Services.Definitions;
using ProvidersPlatform.UserServices.Services.Implementations;
using ProvidersPlatform.UserServices.Services.Models;

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
            opt.Services.AddScoped<IJwtAuthentication, JwtAuthentication>();
            opt.Services.Configure<JwtConfig>(opt.Configuration.GetSection("JwtConfig"));
            
            
            //Inject Databases
            var mariaDbConfig = new MariaDbConfig();
            opt.Configuration.GetSection("Databases:Providers_Platform").Bind(mariaDbConfig);
            opt.Services.AddProvidersPlatformDatabase(mariaDbConfig);

            var sqlServerConfig = new SqlServerConfig();
            opt.Configuration.GetSection("Databases:JwtDatabase").Bind(sqlServerConfig);
            opt.Services.AddDbContext<JwtDbContext>(conf =>
            {
                conf.UseSqlServer(sqlServerConfig.ConnectionString());
            });
            
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

        app.UseAuthentication();
        app.MapControllers();
        app.MapGet("/healthy", ()=> "Rest Api User Healthy");
        
        DefaultApiProject.RunApplication(app);
    }
}