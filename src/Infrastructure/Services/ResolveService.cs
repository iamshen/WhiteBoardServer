using Ardalis.GuardClauses;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class ResolveService : IResolveService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IServiceProvider _serviceProvider;

    public ResolveService(IServiceProvider serviceProvider, IDistributedCache distributedCache)
    {
        _serviceProvider = serviceProvider;
        _distributedCache = distributedCache;
    }

    public async ValueTask<string?> GetCurrentUserId()
    {
        var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
        Guard.Against.Null(httpContextAccessor.HttpContext, nameof(httpContextAccessor.HttpContext));
        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        return await Task.FromResult(userId);
    }

    public ValueTask<UserInfo?> GetCurrentUser()
    {
        var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        return ValueTask.FromResult(httpContextAccessor.HttpContext?.User.GetUserInfo());
    }
}