using Exam.Core.Example.Models.DTO;
using Exam.Core.Example.Models.Entity;
using Exam.Core.Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Core.Example.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    private readonly IExampleService _service;

    public ExampleController(IExampleService service)
    {
        _service = service;
    }
    
    [HttpGet("example")]
    [ProducesResponseType(200)]
    public ActionResult<string> Example([FromQuery] string name)
    {
        return Ok($"Hello {name}!");
    }
    
    [HttpGet("example-service")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<string>> ExampleService([FromServices] IExampleService exampleService)
    {
        return Ok(await exampleService.GetExampleData());
    }

    [HttpPost("example")]
    [ProducesResponseType(typeof(ExampleEntity), StatusCodes.Status200OK)]
    public ActionResult<ExampleEntity> ExampleMapper(ExampleDTO dto)
    {
        var result = _service.ExampleMapping(dto).Result;
    
        return Ok(result);
    }
    
    [HttpPost("customExample")]
    [ProducesResponseType(typeof(ExampleEntity), StatusCodes.Status200OK)]
    public ActionResult<ExampleEntity> CustomExampleMapper(ExampleDTO dto)
    {
        var result = _service.CustomExampleMapping(dto).Result;
    
        return Ok(result);
    }
}