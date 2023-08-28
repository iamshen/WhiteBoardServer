namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     Validation Exception Middleware  Extensions
/// </summary>
public static class ValidationExceptionMiddlewareExtensions
{
    /// <summary>
    ///     Use ValidationException Middleware
    /// </summary>
    /// <param name="app"></param>
    /// <param name="enable"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseValidationException(this IApplicationBuilder app, bool enable = true)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        if (!enable) return app;

        app.UseMiddleware<ValidationExceptionMiddleware>();
        return app;
    }
}