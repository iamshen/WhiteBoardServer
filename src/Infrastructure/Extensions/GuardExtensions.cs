using System.Runtime.CompilerServices;

namespace Ardalis.GuardClauses;

public static class TrueGuard
{
    public static void True(this IGuardClause guardClause, bool input, string message,
        [CallerArgumentExpression("input")] string? parameterName = null)
    {
        if (input)
            throw new ArgumentException(message, parameterName);
    }
}