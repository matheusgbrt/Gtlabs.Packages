using Gtlabs.Api.ApiCall;
using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Authentication.Services;
using Gtlabs.Authentication.Tokens;
using Gtlabs.Consts;

namespace Gtlabs.Authentication.Providers;

public class AppJwtHeaderNormalizer : IHeaderNormalizationProvider
{
    private readonly IAppAuthService _appAuthService;
    private readonly AuthenticationHeaderOptions _options;
    public int Order { get; } = 5;


    public AppJwtHeaderNormalizer(IAppAuthService appAuthService, AuthenticationHeaderOptions options)
    {
        _appAuthService = appAuthService;
        _options = options;
    }

    public async void Normalize(ApiClientCallPrototype prototype)
    {
        if (!_options.UseAuthHeader)
            return;
        var token = await _appAuthService.GetAppToken();
        if (!prototype.Headers.ContainsKey(HeaderFields.Authorization))
        {
            prototype.Headers.Add(HeaderFields.Authorization, $"Bearer {token}");
        }
    }
}