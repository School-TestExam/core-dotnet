using Exam.Abstractions;
using Exam.Core.Example.Configurations;
using Exam.Core.Example.Services;
using Mapster;

namespace Exam.Core.Example;

public class Configuration : ServiceConfiguration
{
    public override void Configure(IApplicationBuilder builder)
    {
        
    }

    public override void ConfigureMapper(TypeAdapterConfig config)
    {
        MapsterConfiguration.Configure();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
    }
}