using System.Reflection;
using Exam.Core.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Exam.Core.Settings;

public class SwaggerConfigurationSettings : IConfigureOptions<SwaggerGenOptions>
{
    private readonly ServiceSettings _settings;
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerConfigurationSettings(IOptions<ServiceSettings> settings, IApiVersionDescriptionProvider provider)
    {
        _settings = settings.Value;
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CustomApiInformation(description));
        }
    }

    private OpenApiInfo CustomApiInformation(ApiVersionDescription description)
    {
        var assembly = Assembly.GetEntryAssembly();

        var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0";

        var apiInfo = new OpenApiInfo
        {
            Title = $"Exam-Test - {_settings.Name}",
            Version = version,
            Description = _settings.Description
        };
        
        if(description.IsDeprecated)
        {
            apiInfo.Description += "This API-version is deprecated!";
        }

        return apiInfo;
    }
}