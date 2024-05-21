using System.Reflection;
using System.Runtime.Loader;
using Exam.Abstractions;
using Exam.Core.Extensions;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Core;

public class Startup
{
    // List all allowed assembly prefixes used to filter .. Security
    private static readonly List<string> allowedPluginAssemblyPrefix = new() { "Exam" };

    // List to store instances of ServiceConfiguration
    private static readonly List<ServiceConfiguration> PluginConfigurations = new();

    // Configuration property for storing application configuration
    public IConfiguration? Configuration { get; init; }

    public Startup()
    {
        LoadPluginAssemblies();
    }

    private static void LoadPluginAssemblies()
    {
        // Get the entry assembly (the main application assembly).
        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly is null)
        {
            throw new("GetEntryAssembly returned null");
        }

        // Load all entry assemblies
        foreach (var asm in entryAssembly.GetReferencedAssemblies())
        {
            try
            {
                AssemblyLoadContext.Default.LoadFromAssemblyName(asm);
            }
            catch
            {
                continue;
            }
        }

        var plugins = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(x => !x.IsDynamic)
            .Where(x => x.FullName is string name && allowedPluginAssemblyPrefix.Any(y => name.StartsWith(y)))
            .SelectMany(x => x.GetExportedTypes())
            .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(ServiceConfiguration)))
            .Distinct()
            .Select(x => (ServiceConfiguration)Activator.CreateInstance(x)!)
            .Where(x => x is not null)
            .ToList();

        if (PluginConfigurations.Any())
        {
            PluginConfigurations.Clear();
        }
        
        PluginConfigurations.AddRange(plugins);
    }

    public virtual void ConfigureApp(IApplicationBuilder builder)
    {
        builder.UseServiceCore();

        ApplyConfigurations(builder, PluginConfigurations);
    }

    public virtual void IntializeServices(IServiceCollection services)
    {
        services.InitializeService(Configuration!);

        ConfigureServices(services, PluginConfigurations);
        ConfigureMapper(services, PluginConfigurations);
    }

    protected virtual void ApplyConfigurations(IApplicationBuilder builder, List<ServiceConfiguration> configurations)
    {
        foreach (var config in configurations)
        {
            config.Configuration = Configuration!;
            config.Configure(builder);
        }
    }
    
    protected virtual void ConfigureServices(IServiceCollection services, List<ServiceConfiguration> configurations)
    {
        foreach (var config in configurations)
        {
            config.Configuration = Configuration!;
            config.ConfigureServices(services);
        }
    }

    protected virtual void ConfigureMapper(IServiceCollection services, List<ServiceConfiguration> configurations)
    {
        var mapperConfig = new TypeAdapterConfig();

        foreach (var config in configurations)
        {
            config.ConfigureMapper(mapperConfig);
        }

        services.AddSingleton(mapperConfig);
    }
}