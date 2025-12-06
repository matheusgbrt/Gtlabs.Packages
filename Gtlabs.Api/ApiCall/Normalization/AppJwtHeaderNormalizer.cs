using Gtlabs.Api.ApiCall.Authentication;
using Gtlabs.Authentication.Services;
using Gtlabs.Authentication.Tokens;
using Gtlabs.Consts;
using Microsoft.Extensions.Options;

namespace Gtlabs.Api.ApiCall.Normalization;

public class AppJwtHeaderNormalizer : IHeaderNormalizationProvider
{
    private readonly IOptions<AuthenticationHeaderOptions> _options;
    private readonly IAppAuthService _appAuthService;
    private readonly IAuthenticationApiCall _authenticationApi;
    public int Order { get; } = 5;


    public AppJwtHeaderNormalizer(IAppAuthService appAuthService, IOptions<AuthenticationHeaderOptions> options, IAuthenticationApiCall authenticationApi)
    {
        _appAuthService = appAuthService;
        _options = options;
        _authenticationApi = authenticationApi;
    }

    public async void Normalize(ApiClientCallPrototype prototype)
    {
        if (!_options.Value.UseAuthHeader)
            return;
        var token = await _appAuthService.GetAppToken();
        if (string.IsNullOrEmpty(token))
        {
            token = await _authenticationApi.RequestAppToken();
        }
        await _appAuthService.SetAppToken(token);
        if (!prototype.Headers.ContainsKey(HeaderFields.Authorization))
        {
            prototype.Headers.Add(HeaderFields.Authorization, $"Bearer {token}");
        }
    }
}