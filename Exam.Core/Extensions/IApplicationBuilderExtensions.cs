using Exam.Abstractions.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Exam.Core.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseServiceCore(this IApplicationBuilder builder)
    {
        var include = builder.ApplicationServices
            .GetRequiredService<IOptions<IncludeSettings>>().Value;

        if (include.Mvc)
        {
            builder.UseResponseCaching();

            builder.UseRouting();

            builder.UseEndpoints(options => { options.MapControllers(); });
        }

        if (include.Swagger)
        {
            builder.UseSwagger();
        }

        return builder;
    }

    private static void UseSwagger(this IApplicationBuilder builder)
    {
        var options = builder.ApplicationServices.GetRequiredService<IOptions<ServiceSettings>>().Value;
        var apiProvider = builder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        builder.UseSwagger(config =>
        {
            config.RouteTemplate = "api/swagger/{documentName}/swagger.json";

            if (!options.Debug)
            {
                config.PreSerializeFilters.Add((swagger, _) =>
                {
                    var paths = new OpenApiPaths();

                    foreach (var path in swagger.Paths)
                    {
                        paths.Add(path.Key.Replace("/api/", $"/{options.Route}/"), path.Value);
                        
                    }

                    swagger.Paths = paths;
                });
            }
        });

        builder.UseSwaggerUI(config =>
        {
            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                var title = $"Exam.Test - {options.Name} {description.GroupName}";

                if (options.Debug)
                {
                    config.SwaggerEndpoint($"/api/swagger/{description.GroupName}/swagger.json", title);
                }
                else
                {
                    config.SwaggerEndpoint($"/{options.Route}/swagger/{description.GroupName}/swagger.json", title);
                }
                
            }
            
            config.RoutePrefix = "api/swagger";
        });
    }
}