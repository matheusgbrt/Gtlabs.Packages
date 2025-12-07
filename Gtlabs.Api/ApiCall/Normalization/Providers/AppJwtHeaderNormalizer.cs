using Gtlabs.Api.ApiCall.Authentication;
using Gtlabs.Authentication.Tokens;
using Gtlabs.Consts;
using Microsoft.Extensions.Options;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class AppJwtHeaderNormalizer : IHeaderNormalizationProvider
{
    private readonly IOptions<AuthenticationHeaderOptions> _options;
    private readonly ITokenProvider _tokenProvider;

    public int Order => 5;

    public AppJwtHeaderNormalizer(ITokenProvider tokenProvider, IOptions<AuthenticationHeaderOptions> options)
    {
        _tokenProvider = tokenProvider;
        _options = options;
    }

    public async Task Normalize(ApiClientCallPrototype prototype)
    {
        if (!_options.Value.UseAuthHeader)
            return;
        
        if (prototype.SkipAuthHeader)
            return;

        var token = await _tokenProvider.GetOrRefreshTokenAsync();

        if (!prototype.Headers.ContainsKey(HeaderFields.Authorization))
        {
            prototype.Headers.Add(HeaderFields.Authorization, $"Bearer {token}");
        }
    }
}