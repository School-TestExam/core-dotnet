using Exam.Abstractions.Settings;

namespace Exam.Core.Extensions;

public class ConfigurationSettings
{
    public ServiceSettings Service { get; set; } = new();
    public IncludeSettings Include { get; set; } = new();
}