using System.Net;
using API.Models;
using Application.Exceptions;
using Newtonsoft.Json;

namespace API.Middlewares;
public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
    CustomValidationProblemDetails problem = new();

    switch (ex)
    {
      case BadRequestException badRequestException:
        statusCode = HttpStatusCode.BadRequest;
        problem = new CustomValidationProblemDetails
        {
          Title = badRequestException.Message,
          Status = (int)statusCode,
          Detail = badRequestException.InnerException?.Message,
          Type = nameof(badRequestException),
          Errors = badRequestException.ValidadtionErrors
        };
        break;
      case NotFoundException notFound:
        statusCode = HttpStatusCode.NotFound;
        problem = new CustomValidationProblemDetails
        {
          Title = notFound.Message,
          Status = (int)statusCode,
          Type = nameof(NotFoundException),
          Detail = notFound.InnerException?.Message,
        };
        break;
      default:
        problem = new CustomValidationProblemDetails
        {
          Title = ex.Message,
          Status = (int)statusCode,
          Type = nameof(HttpStatusCode.InternalServerError),
          Detail = ex.StackTrace
        };
        break;
    }

    context.Response.StatusCode = (int)statusCode;
    var logMessage = JsonConvert.SerializeObject(problem);
    _logger.LogError(logMessage);
    await context.Response.WriteAsJsonAsync(problem);
  }
}
