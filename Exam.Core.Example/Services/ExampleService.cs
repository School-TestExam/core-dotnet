using Exam.Core.Example.Models.DTO;
using Exam.Core.Example.Models.Entity;
using Mapster;

namespace Exam.Core.Example.Services;

public interface IExampleService
{
    public Task<string> GetExampleData();
    public Task<ExampleEntity> ExampleMapping(ExampleDTO dto);
    public Task<CustomExampleEntity> CustomExampleMapping(ExampleDTO dto);
}

public class ExampleService : IExampleService
{
    public async Task<string> GetExampleData()
    {
        await Task.Delay(250);

        return "This is example data!";
    }

    public Task<ExampleEntity> ExampleMapping(ExampleDTO dto)
    {
        var entity = dto.Adapt<ExampleEntity>();

        return Task.FromResult(entity);
    }

    public Task<CustomExampleEntity> CustomExampleMapping(ExampleDTO dto)
    {
        var entity = dto.Adapt<CustomExampleEntity>();

        return Task.FromResult(entity);
    }
}