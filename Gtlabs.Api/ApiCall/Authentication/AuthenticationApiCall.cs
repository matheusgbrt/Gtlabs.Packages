using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Api.ApiCall.Tokens;
using Gtlabs.Consts;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gtlabs.Api.ApiCall.Authentication;

public class AuthenticationApiCall : IAuthenticationApiCall, ITransientDependency
{
    private readonly IServiceProvider _provider;
    private readonly string _authServiceName = "GTLabs.Identity.Authentication";
    private readonly string _authPath = "/identity/authentication/service";
    private readonly string _appId;

    public AuthenticationApiCall( IAmbientData ambientData, IServiceProvider provider)
    {
        _provider = provider;
        _appId = ambientData.GetAppId();
    }

    public async Task<string> RequestAppToken()
    {
        var builder = _provider.GetRequiredService<IApiClientCallBuilder>();
        var request = 
            await builder
                .WithServiceName(_authServiceName)
                .WithMethod(HttpMethod.Get)
                .WithUrl($"{_authPath}/{_appId}")
                .WithTimeout(CallTimeout.Long)
                .WithoutAuthHeader()
                .WithHeader(HeaderFields.ServiceKey, "")
                .ExecuteSafeAsync();

        if (request.Success)
        {
            var token = request.MapTo<string>();

            return token;
        }
        
        return "";
    }
}