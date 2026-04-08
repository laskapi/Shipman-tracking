using shipman.Server.Application.Exceptions;
using System.Net;

namespace shipman.Server.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case AppValidationException vex:
                    _logger.LogInformation(
                        "Validation rejected: {Summary}",
                        string.Join(
                            " | ",
                            vex.Errors.SelectMany(p => p.Value.Select(v => $"{p.Key}: {v}"))));
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { errors = vex.Errors });
                    break;

                case AppNotFoundException nfex:
                    _logger.LogInformation("Not found: {Message}", nfex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { nfex.Message } }
                    });
                    break;

                case AppDomainException dex:
                    _logger.LogInformation("Domain rule rejected: {Message}", dex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { dex.Message } }
                    });
                    break;
                case AppInvalidOperationException ioex:
                    _logger.LogInformation("Conflict: {Message}", ioex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { ioex.Message } }
                    });
                    break;
                default:
                    _logger.LogError(ex, "Unhandled exception");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { "Unexpected server error" } }
                    });
                    break;
            }
        }
    }
}
