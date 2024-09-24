using Catalog.API.Exceptions;

namespace Catalog.API;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred: {ex.Message}");

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case ProductNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var response = new
            {
                message = ex.Message,
                error = ex.GetType().Name
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}