using System.ComponentModel.DataAnnotations;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketServer;

/// <summary>
///     Result Extensions
/// </summary>
/// <remarks>扩展信息: 成功返回啥,失败返回啥</remarks>
public static class ResultExtensions
{
    /// <summary>
    ///     ToOkResult
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static IResult ToOkResult<TResult, TOutput>(this Result<TResult> result, Func<TResult, TOutput> mapper)
    {
        return result.Match<IResult>(res =>
        {
            var response = mapper(res);
            return Results.Ok(response);
        }, ex =>
        {
            if (ex is ValidationException validationException)
                return Results.BadRequest(validationException.ToProblemDetails());

            // TODO: Others Exceptions

            return Results.BadRequest(ex.Message);
        });
    }

    /// <summary>
    ///     ToCreatedResult
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <param name="uri"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static IResult ToCreatedResult<TResult, TOutput>(this Result<TResult> result, Func<TResult, TOutput> mapper,
        string uri)
    {
        return result.Match<IResult>(res =>
        {
            var response = mapper(res);
            return Results.Created(uri, response);
        }, ex =>
        {
            if (ex is ValidationException validationException)
                return Results.BadRequest(validationException.ToProblemDetails());

            // TODO: Others Exceptions

            return Results.BadRequest(ex.Message);
        });
    }
}