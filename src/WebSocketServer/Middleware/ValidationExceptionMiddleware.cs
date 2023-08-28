using System.ComponentModel.DataAnnotations;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     Validation Exception Middleware
/// </summary>
public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _request;

    public ValidationExceptionMiddleware(RequestDelegate request)
    {
        _request = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            var error = ex.ToProblemDetails();
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}