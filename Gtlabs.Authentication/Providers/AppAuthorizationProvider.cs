using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Api.ApiCall;
using Gtlabs.Api.ApiCall.Tokens;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;

namespace Gtlabs.Authentication.Providers;

public class AppAuthorizationProvider : IAppAuthorizationProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private readonly string _authServiceAppName;
    private readonly string _baseEndpoint = "identity/authorization/apps/{0}/permissions";    
    
    public AppAuthorizationProvider(IApiClientCallBuilder apiClientCallBuilder, IAmbientData ambientData)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _authServiceAppName = ambientData.GetAppId();
    }

    public async Task<ApiResponseEnvelope> GetAppPermissionAsync(string appIdentifier)
    {
        var response =
            await _apiClientCallBuilder
                .WithServiceName(_authServiceAppName)
                .WithMethod(HttpMethod.Get)
                .WithUrl(string.Format(_baseEndpoint, appIdentifier))
                .WithHeader("X-UserID","teste")
                .WithTimeout(CallTimeout.Long)
                .ExecuteSafeAsync();

        return response;
    }
}