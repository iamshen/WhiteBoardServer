using System.ComponentModel.DataAnnotations;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     Validation Exception Extensions
/// </summary>
public static class ValidationExceptionExtensions
{
    public static ValidationProblemDetails ToProblemDetails(this ValidationException ex)
    {
        var error = new ValidationProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Status = 400
        };

        foreach (var validationFailure in ex.ValidationResult.MemberNames)
            if (!string.IsNullOrEmpty(ex.ValidationResult.ErrorMessage))
                error.Errors.Add(new KeyValuePair<string, string[]>(
                    validationFailure,
                    new[] { ex.ValidationResult.ErrorMessage }
                ));

        return error;
    }
}