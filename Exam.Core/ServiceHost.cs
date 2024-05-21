using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Exam.Core;

public static class ServiceHost<TStartup> where TStartup : Startup, new()
{
    public static int Run(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        //TODO: Potentially Implement logging

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var startup = new TStartup
            {
                Configuration = builder.Configuration
            };
            
            startup.IntializeServices(builder.Services);

            var app = builder.Build();
            
            startup.ConfigureApp(app);

            switch (args.Any() ? args[0] : string.Empty)
            {
                case "migrate":
                    //TODO: Implement cloud based migration if nesseccary
                    break;
                
                default:
                    app.Run();
                    break;
            }
            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex} A fatal error occurred while executing host");
        }

        return 0;
    }
}

public static class ServiceHost
{
    public static int Run(string[] args)
    {
        return ServiceHost<Startup>.Run(args);
    }
} 