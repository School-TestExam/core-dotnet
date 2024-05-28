namespace Exam.Core.Extensions;

public class ServiceSettings
{
    public bool Debug { get; set; } = false;
    public string? Description { get; set; } = default;
    public string Name { get; set; } = default;
    public string Route { get; set; } = default;
}