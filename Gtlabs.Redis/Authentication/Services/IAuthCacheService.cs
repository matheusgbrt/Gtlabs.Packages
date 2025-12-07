using Gtlabs.Redis.Authentication.Entities;

namespace Gtlabs.Redis.Authentication.Services;

public interface IAuthCacheService
{
    Task<string> GetCachedServiceToken(string appId);
    Task SetCachedServiceToken(CachedServiceJwt cachedServiceJwt);
}