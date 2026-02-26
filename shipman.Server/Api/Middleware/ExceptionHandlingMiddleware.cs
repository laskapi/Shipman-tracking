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
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case AppValidationException vex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { errors = vex.Errors });
                    break;

                case AppNotFoundException nfex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { nfex.Message } }
                    });
                    break;

                case AppDomainException dex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        errors = new { General = new[] { dex.Message } }
                    });
                    break;

                default:
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
