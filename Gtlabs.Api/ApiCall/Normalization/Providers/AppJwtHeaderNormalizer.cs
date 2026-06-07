using Gtlabs.Api.ApiCall.Authentication;
using Gtlabs.Consts;
using Gtlabs.Consts.Authentication;
using Microsoft.Extensions.Options;

namespace Gtlabs.Api.ApiCall.Normalization.Providers;

public class AppJwtHeaderNormalizer : IHeaderNormalizationProvider
{
    private readonly IOptions<AuthenticationHeaderOptions> _authenticationOptions;
    private readonly IOptions<AppTokenHydrationOptions> _appTokenOptions;
    private readonly IAppTokenProvider _appTokenProvider;

    public AppJwtHeaderNormalizer(
        IOptions<AuthenticationHeaderOptions> authenticationOptions,
        IOptions<AppTokenHydrationOptions> appTokenOptions,
        IAppTokenProvider appTokenProvider)
    {
        _authenticationOptions = authenticationOptions;
        _appTokenOptions = appTokenOptions;
        _appTokenProvider = appTokenProvider;
    }

    public int Order => 5;
    
    public async Task Normalize(ApiClientCallPrototype prototype)
    {
        if (!_authenticationOptions.Value.UseAuthHeader)
            return;
        
        if (prototype.SkipAuthHeader)
            return;

        if (IsAuthorizationServiceCall(prototype))
            return;

        if (!prototype.Headers.ContainsKey(HeaderFields.Authorization))
        {
            var token = await _appTokenProvider.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return;

            prototype.Headers.Add(HeaderFields.Authorization, $"Bearer {token}");
        }
    }

    private bool IsAuthorizationServiceCall(ApiClientCallPrototype prototype)
        => string.Equals(
            prototype.ServiceName,
            _appTokenOptions.Value.AuthorizationServiceName,
            StringComparison.OrdinalIgnoreCase);
}
