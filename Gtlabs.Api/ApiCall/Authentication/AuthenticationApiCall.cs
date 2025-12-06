using Gtlabs.Api.ApiCall.Tokens;
using Gtlabs.Consts;
using Gtlabs.Core.AmbientData.Interfaces;
using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;

namespace Gtlabs.Api.ApiCall.Authentication;

public class AuthenticationApiCall : IAuthenticationApiCall, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly string _authServiceName = "GTLabs.Identity.Authentication";
    private readonly string _authPath = "/identity/authentication/service";
    private readonly string _appId;

    public AuthenticationApiCall(IApiClientCallBuilder apiClientCallBuilder, IAmbientData ambientData)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _appId = ambientData.GetAppId();
    }

    public async Task<string> RequestAppToken()
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

            return token;
        }
        
        return "";
    }
}