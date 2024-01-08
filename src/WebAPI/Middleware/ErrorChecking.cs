using Domain.Exceptions;
using Infrastructure.Models;
using System.Data.Common;
using System.Security;

namespace WebAPI.Middleware;

/// <summary>
/// This class provides error checking functionality.
/// </summary>
public class ErrorChecking
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorChecking> _logger;

    /// <summary>
    /// Initializes a new instance of the ErrorChecking class.
    /// </summary>
    /// <param name="next">The request delegate.</param>
    /// <param name="logger">The logger.</param>
    public ErrorChecking(RequestDelegate next, ILogger<ErrorChecking> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to perform error checking asynchronously.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            string message;
            int statusCode;

            switch (e)
            {
                case FoundException:
                    message = "All ready exists";
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case UnauthorizedAccessException:
                    message = "Use higher authorization level";
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case NotImplementedException:
                    message = "Not implemented";
                    statusCode = StatusCodes.Status501NotImplemented;
                    break;
                case SecurityException:
                    message = "Authentication error";
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case NullReferenceException:
                    message = "Null reference caught";
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case ArgumentException:
                    message = "Argument is incorrect";
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                    message = "Entity not found";
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case DbException:
                    message = "Database error";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
                default:
                    message = "General error";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            ErrorViewModel errorMessage = new(message, statusCode, e);
            await UpdateContextAndLog(errorMessage, context);
        }
    }

    private async Task UpdateContextAndLog(ErrorViewModel errorMessage, HttpContext context)
    {
        context.Response.StatusCode = errorMessage.StatusCode;

        _logger.LogError("{Message}", errorMessage.Message);
        await context.Response.WriteAsJsonAsync(errorMessage);
    }
}
