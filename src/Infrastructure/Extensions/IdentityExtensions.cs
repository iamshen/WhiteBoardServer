using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using Infrastructure.ValueObjects;

namespace System;

/// <summary> Identity Extensions </summary>
public static class IdentityExtensions
{
    public static string? GetUserId(this ClaimsPrincipal? user)
    {
        var identity = user?.Identity;
        return identity is not ClaimsIdentity claimsIdentity
            ? default
            : claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static UserInfo? GetUserInfo(this ClaimsPrincipal? user)
    {
        var identity = user?.Identity;

        if (identity is not ClaimsIdentity claimsIdentity)
            return default;

        var userId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return default;

        var name = claimsIdentity!.FindFirst(JwtClaimTypes.Name)?.Value;
        var nickName = claimsIdentity!.FindFirst(JwtClaimTypes.NickName)?.Value;
        var phone = claimsIdentity!.FindFirst(JwtClaimTypes.PhoneNumber)?.Value;
        var email = claimsIdentity!.FindFirst(JwtClaimTypes.Email)?.Value;
        var avatar = claimsIdentity!.FindFirst(JwtClaimTypes.Picture)?.Value;
        var address = claimsIdentity!.FindFirst(JwtClaimTypes.Address)?.Value;

        return new UserInfo
        {
            Id = userId,
            NickName = nickName,
            PhoneNumber = phone,
            Email = email,
            Avatar = avatar,
            IsAdmin = false,
            Address = string.IsNullOrWhiteSpace(address) ? default : JsonSerializer.Deserialize<Address>(address)
        };
    }
}