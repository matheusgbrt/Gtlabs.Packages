using Gtlabs.Authentication.Entities;
using Gtlabs.Core.AmbientData.Interfaces;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;

namespace Gtlabs.Authentication.Services;

public class AppAuthService : IAppAuthService, ITransientDependency
{
    private readonly IAuthCacheService _authCacheService;
    private readonly string _appId;
    
    public AppAuthService(IAuthCacheService authCacheService, IServiceProvider provider, IAmbientData ambientData)
    {
        _authCacheService = authCacheService;
        _appId = ambientData.GetAppId();
    }

    public async Task<string> GetAppToken()
    {
        var cachedToken = await _authCacheService.GetCachedServiceToken(_appId);
        return cachedToken;
    }
    
    public async Task SetAppToken(string token)
    {
        var cachedToken = new CachedServiceJwt(_appId)
        {
            Token = token
        };
        await _authCacheService.SetCachedServiceToken(cachedToken);

    }

    
}