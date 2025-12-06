using Gtlabs.Authentication.Services;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;

namespace Gtlabs.Api.ApiCall.Authentication;

public class TokenProvider : ITokenProvider, IScopedDependency
{
    private readonly IAppAuthService _cache;
    private readonly IAuthenticationApiCall _authApi;

    public TokenProvider(IAppAuthService cache, IAuthenticationApiCall authApi)
    {
        _cache = cache;
        _authApi = authApi;
    }

    public async Task<string> GetOrRefreshTokenAsync()
    {
        var token = await _cache.GetAppToken();
        if (string.IsNullOrEmpty(token))
            token = await _authApi.RequestAppToken();

        await _cache.SetAppToken(token);
        return token;
    }
}