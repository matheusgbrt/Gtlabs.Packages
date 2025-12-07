using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Api.ApiCall.Authentication;
using Gtlabs.Consts;
using Gtlabs.Consts.Authentication;
using Gtlabs.Redis.Authentication.Services;
using Microsoft.Extensions.Options;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class AppJwtHeaderNormalizer : IHeaderNormalizationProvider
{
    private readonly IOptions<AuthenticationHeaderOptions> _options;
    private readonly IAuthCacheService _authCacheService;
    private readonly IAmbientData _ambientData;

    public AppJwtHeaderNormalizer(IOptions<AuthenticationHeaderOptions> options, IAuthCacheService authCacheService, IAmbientData ambientData)
    {
        _options = options;
        _authCacheService = authCacheService;
        _ambientData = ambientData;
    }

    public int Order => 5;
    
    public async Task Normalize(ApiClientCallPrototype prototype)
    {
        if (!_options.Value.UseAuthHeader)
            return;
        
        if (prototype.SkipAuthHeader)
            return;

        var token = await _authCacheService.GetCachedServiceToken(_ambientData.GetAppId());

        if (!prototype.Headers.ContainsKey(HeaderFields.Authorization))
        {
            prototype.Headers.Add(HeaderFields.Authorization, $"Bearer {token}");
        }
    }
}