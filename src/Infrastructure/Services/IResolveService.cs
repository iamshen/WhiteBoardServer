using Infrastructure.ValueObjects;

namespace Infrastructure.Services;

public interface IResolveService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ValueTask<string?> GetCurrentUserId();
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ValueTask<UserInfo?> GetCurrentUser();
}