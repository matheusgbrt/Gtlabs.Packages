using System.Net.Http;
using System.Threading.Tasks;
using Gtlabs.Api.AmbientData.Interfaces;
using Gtlabs.Api.ApiCall;
using Gtlabs.Api.ApiCall.Tokens;
using Gtlabs.Consts;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;

namespace Gtlabs.Authentication.Services;

public class AppAuthService : IAppAuthService, ITransientDependency
{
    private readonly IAuthCacheService _authCacheService;
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private readonly string _authServiceName = "GTLabs.Identity.Authentication";
    private readonly string _authPath = "/identity/authentication/auth-service";
    private readonly string _appId;
    
    
    public AppAuthService(IAuthCacheService authCacheService, IApiClientCallBuilder apiClientCallBuilder, IAmbientData ambientData)
    {
        _authCacheService = authCacheService;
        _apiClientCallBuilder = apiClientCallBuilder;
        _appId = ambientData.GetAppId();
    }

    public async Task<string> GetAppToken()
    {
        var cachedToken = await _authCacheService.GetCachedServiceToken(_appId);
        if (!string.IsNullOrEmpty(cachedToken))
            return cachedToken;
        return await RequestAppToken();
    }

    private async Task<string> RequestAppToken()
    {
        var request = 
            await _apiClientCallBuilder
                .WithServiceName(_authServiceName)
                .WithMethod(HttpMethod.Get)
                .WithUrl($"{_authPath}/{_appId}")
                .WithTimeout(CallTimeout.Long)
                .WithHeader(HeaderFields.ServiceKey, "")
                .ExecuteSafeAsync();

        if (request.Success)
        {
            var token = request.MapTo<string>();
            var cachedToken = new CachedServiceJwt(_appId)
            {
                Token = token,
            };
            return token;
        }
        
        return "";
    }
    
}