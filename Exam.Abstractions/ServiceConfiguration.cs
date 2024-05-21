using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Abstractions;

public abstract class ServiceConfiguration
{
    public IConfiguration Configuration { get; set; } = default!;

    public abstract void Configure(IApplicationBuilder builder);

    public virtual void ConfigureMapper(TypeAdapterConfig config) { }

    public abstract void ConfigureServices(IServiceCollection services);
}