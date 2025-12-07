using System.Threading.Tasks;
using Gtlabs.Authentication.Entities;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Gtlabs.Redis.Interfaces;

namespace Gtlabs.Authentication.Services;

public class AuthCacheService : IAuthCacheService, ITransientDependency
{
    private readonly ICacheService<CachedServiceJwt> _cacheService;

    public AuthCacheService(ICacheService<CachedServiceJwt> cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<string> GetCachedServiceToken(string appId)
    {
        var cachedServiceJwt = new CachedServiceJwt(appId);
        var cachedToken = await _cacheService.GetAsync(cachedServiceJwt);
        if (cachedToken == null || string.IsNullOrEmpty(cachedToken.Token))
        {
            return "";
        }
        return cachedToken!.Token;
    }

    public async Task SetCachedServiceToken(CachedServiceJwt cachedServiceJwt)
    {
        await _cacheService.SetAsync(cachedServiceJwt,TimeSpan.FromMinutes(50));
    }
}