using GraphQL;
using ProvidersPlatform.PostServices.GraphQl.Graphs;
using ProvidersPlatform.PostServices.GraphQl.Graphs.Queries;
using ProvidersPlatform.Shared.Setup;
using ProvidersPlatform.Shared.Setup.Configs;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.PostServices.GraphQl;

public class Program
{
    public static void Main(string[] args)
    {
        var app = DefaultApiProject.GenerateWebApplicationTemplate(opt =>
        {
            
            //Injection to Providers Platform Database
            var mariaDbConfig = new MariaDbConfig();
            opt.Configuration.GetSection("Databases:Providers_Platform").Bind(mariaDbConfig);
            opt.Services.AddProvidersPlatformDatabase(mariaDbConfig);
            
            //Injection to custom services
            opt.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            //Injection to GraphQL library
            opt.Services.AddGraphQL(conf =>
            {
                conf.AddSchema<DefaultSchema>();
                conf.AddSystemTextJson();
            });
            
            //Injection to Graph Queries
            opt.Services.AddSingleton<PostQueries>();
            opt.Services.AddSingleton<RootQuery>();
        });

        app.UseAuthorization();
        app.UseGraphQL<DefaultSchema>();
        app.MapGet("/healthy", () => "GraphQl Posts Api Are Healthy");
        
        DefaultApiProject.RunApplication(app);
    }
}