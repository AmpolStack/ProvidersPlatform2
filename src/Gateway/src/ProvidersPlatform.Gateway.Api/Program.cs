using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.Gateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            
            opt.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            //Inject Jwt Authentication
            opt.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(conf =>
                {
                    conf.RequireHttpsMetadata = false;
                    conf.SaveToken = true;
                
                    conf.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Configuration["JwtConfig:Key"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            opt.Services.AddOcelot();
            
        });
        app.Use(async (ctx, next) =>
        {
            var claims = ctx.User.Claims;
            await next();
            var claims2 = ctx.User.Claims;
            var x = "";
        });
        app.UseAuthentication();
        app.UseOcelot();
        app.MapGet("/healthy", () => "Api Gateway Healthy");

        DefaultApiProject.RunApplication(app);
    }
}