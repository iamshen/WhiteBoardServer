using System.Security.Claims;
using Ardalis.GuardClauses;
using Infrastructure.Constants;
using Infrastructure.Dtos.Room;
using Infrastructure.Jwt;
using Infrastructure.Services;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using WebSocketServer;
using WebSocketServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

#region redis

var redisCacheOptions = builder.Configuration.GetSection(nameof(RedisCacheOptions)).Get<RedisCacheOptions>();
Guard.Against.Null(redisCacheOptions, nameof(redisCacheOptions));
builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = redisCacheOptions.Configuration;
    opts.InstanceName = redisCacheOptions.InstanceName;
});

#endregion

#region services

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IResolveService, ResolveService>();
builder.Services.TryAddScoped<IOnlineUserProvider, OnlineUserProvider>();
builder.Services.TryAddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.TryAddSingleton<IConnectionUserCache, ConnectionUserCache>();
builder.Services.AddHttpContextAccessor();

#endregion

#region Cors

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin();
        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowAnyMethod();
    });
});

#endregion

#region AddAuthentication

builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        IdentityModelEventSource.ShowPII = true;
        options.Authority = " https://localhost:44300";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = OnChallenge(),
            OnAuthenticationFailed = OnAuthenticationFailed()
        };
    });
builder.Services.AddAuthorization();

#endregion

#region Signalr

builder.Services.AddSignalR();
builder.Services.Configure<WebSocketOptions>(o => o.KeepAliveInterval = TimeSpan.FromMinutes(2.0));

#endregion

var app = builder.Build();

app.UseValidationException(false);
app.UseFileServer();
app.UseRouting();

app.UseAuthentication();
app.UseCors();
app.UseAuthorization();
app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromMinutes(2) });

#region Hub Endpoints

app.MapHub<WhiteBoardHub>(Endpoints.WhiteBoardHub).RequireAuthorization();

#endregion

#region Room Endpoint

app.MapPost(Endpoints.CreateRoom, CreateRoomAsync).RequireAuthorization();
app.MapGet(Endpoints.GetRoom, GetRoomAsync).RequireAuthorization();

#endregion

app.Run();

return;

#region RoomApi

async Task<IResult> CreateRoomAsync([FromServices] IRoomService service, [FromServices] IJwtService jwtService,
    [FromBody] CreateInputDto input)
{
    var result = await service.CreateAsync(input);

    return result.ToCreatedResult(outputDto =>
    {
        var token = jwtService.CreateToken(new Claim(JwtConstants.Claims.RoomId, outputDto.RoomId));
        return new RoomTokenObject
        {
            RoomId = outputDto.RoomId,
            RoomToken = token
        };
    }, Endpoints.GetRoom);
}

async Task<IResult> GetRoomAsync([FromServices] IRoomService service, string id)
{
    var result = await service.GetAsync(id);

    return result.ToOkResult(roomOutputDto => roomOutputDto);
}

#endregion

#region JwtBearerEvents

Func<AuthenticationFailedContext, Task> OnAuthenticationFailed()
{
    return context =>
    {
        // 打印些日志看在错误是什么
        Console.WriteLine("On Authentication Failed: {0}", context.Exception.Message);
        Console.WriteLine("On Authentication Failed Inner Message: {0}", context.Exception.InnerException?.Message);
        return Task.CompletedTask;
    };
}

Func<JwtBearerChallengeContext, Task> OnChallenge()
{
    return context =>
    {
        // 打印些日志看在错误是什么
        Console.WriteLine("On Challenge Failed: {0}", context.Error);
        Console.WriteLine("On Challenge Failed: {0}", context.AuthenticateFailure?.Message);
        Console.WriteLine("On Challenge Failed: {0}", context.AuthenticateFailure?.InnerException?.Message);
        return Task.CompletedTask;
    };
}

#endregion

#region Endpoints

/// <summary>
///     System Endpoints
/// </summary>
internal static class Endpoints
{
    public const string WhiteBoardHub = "/whiteboard";
    public const string CreateRoom = "/api/rooms";
    public const string GetRoom = "/api/room/{id}";
}

#endregion