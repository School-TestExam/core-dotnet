using Exam.Core.Example.Models.DTO;
using Exam.Core.Example.Models.Entity;
using Mapster;

namespace Exam.Core.Example.Configurations;

public class MapsterConfiguration
{
    public static void Configure()
    {
        // NOTE: Mapster does reverse mapping by default!
        TypeAdapterConfig<ExampleDTO, CustomExampleEntity>.NewConfig()
            .Map(dst => dst.This, src => src.Name)
            .Map(dst => dst.Is, src => src.Age)
            .Map(dst => dst.Mapped, src => src.Active);
    }
}