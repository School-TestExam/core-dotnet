namespace Exam.Abstractions.Settings;

public class IncludeSettings
{
    public bool Mvc { get; set; } = true;
    public bool Swagger { get; set; } = true;
    public bool Versioning { get; set; } = true;
    public bool Mapper { get; set; } = true;
}