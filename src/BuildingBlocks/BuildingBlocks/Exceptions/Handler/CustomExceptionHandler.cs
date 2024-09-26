using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

// class library IExceptionHandler alamaz göremez çünkü -- bunun için fluent val. ve fluent.val.asp yüklenmeli böylece gelir lazım :Microsoft.AspNetCore.Diagnostics
public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            $"Error message: {exception.Message}, time of occurrence: {DateTime.UtcNow}");

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            BadRequestException => 
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            NotFoundException => 
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            ValidationException => 
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            FluentValidation.ValidationException => 
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            _ => 
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };
        
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path,
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is FluentValidation.ValidationException validationException)
        {
            // Add ValidationErrors to the extensions
            var validationErrors = validationException.Errors
                .Select(failure => new { failure.PropertyName, failure.ErrorMessage })
                .ToList();

            problemDetails.Extensions.Add("ValidationErrors", validationErrors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}