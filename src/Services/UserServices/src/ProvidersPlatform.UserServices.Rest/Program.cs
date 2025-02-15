using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Configs;
using ProvidersPlatform.Shared.Setup.Contexts;
using ProvidersPlatform.Shared.Setup.Defaults;

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
                
            //Inject Databases
            var mariaDbConfig = new MariaDbConfig();
            opt.Configuration.GetSection("Databases:Providers_Platform").Bind(mariaDbConfig);
            opt.Services.AddProvidersPlatformDatabase(mariaDbConfig);
            
        });
        
        app.MapControllers();
        app.MapGet("/healthy", ()=> "Rest Api User Healthy");
        app.MapGet("/gt", async ([FromServices] ProvidersPlatformContext ctx) => await ctx.Users.Select(u => new {u.UserId, u.Name, u.UserType, u.Email}).FirstAsync(x => x.UserId == 3));
        app.MapGet("/ls", async ([FromServices] ProvidersPlatformContext ctx) =>
        {
            var newUser = new User()
            {
                Name = "TestUserLam2",
                Address = "Calle 12 #442",
                Description = "TestUserLam Description2",
                Email = "TestUserLam@gmail.com2",
                Password = "confir3222",
                UserType = "provider"
            };
            var provider = new Provider()
            {
                Nit = 1390,
                AssociationPrefix = "LTDA",
                EntityName = "LambaFixed2",
                User = newUser
            };
            var resp = await ctx.Providers.AddAsync(provider);
            await ctx.SaveChangesAsync();
            return "todo salió excelente papacho";
        });
        DefaultApiProject.RunApplication(app);
    }
}