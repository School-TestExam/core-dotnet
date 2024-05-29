using System.Net;
using Exam.Abstractions.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exam.Core.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationException:
            {
                ValidationResult result = new(validationException.Errors);
                result.AddToModelState(context.ModelState, null);

                context.Result = new BadRequestObjectResult(context.ModelState);
                break;
            }

            case AggregateException ae when ae.InnerExceptions.Any(o => o is TaskCanceledException || o is OperationCanceledException):
            case TaskCanceledException:
            case OperationCanceledException:
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                context.Result = GetResult(context);
                break;
            }

            case NotFoundException:
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Result = GetResult(context);
                break;
            }

            case BadRequestException:
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = GetResult(context);
                break;
            }

            default:
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = GetResult(context);
                break;
            }
        }
    }

    private IActionResult GetResult(ExceptionContext context)
    {
        if (context.Exception.GetType().IsAssignableTo(typeof(BaseException)))
        {
            return new JsonResult(new
            {
                Title = ((HttpStatusCode)context.HttpContext.Response.StatusCode).ToString(),
                Status = context.HttpContext.Response.StatusCode,
                Message = context.Exception.InnerException is not null ? context.Exception.InnerException.Message : context.Exception.Message
            });
        }
   
        return new JsonResult(new
        {
            Code = context.Exception.InnerException is not null ? context.Exception.InnerException.Message : context.Exception.Message,
            Stacktrace = context.Exception.StackTrace
        });
    }
}