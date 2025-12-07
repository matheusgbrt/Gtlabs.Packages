using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Api.ApiCall.Normalization;
using Gtlabs.Api.ApiCall.Tokens;

namespace Gtlabs.Api.ApiCall;

public class ApiClientCallBuilder : IApiClientCallBuilder
{
    private ApiClientCallPrototype _prototype;
    private IApiClientCall _call;
    private readonly IAmbientData _ambientData;
    private readonly ApiHeaderNormalizationService _normalizationService;



    public ApiClientCallBuilder(IAmbientData ambientData, ApiHeaderNormalizationService normalizationService, IApiClientCall call)
    {
        _ambientData = ambientData;
        _normalizationService = normalizationService;
        _call = call;
        _prototype = new ApiClientCallPrototype();
    }

    public IApiClientCallBuilder WithUrl(string path)
    {
        var gateway = _ambientData.GetGatewayUrl().TrimEnd('/');
        path = path.TrimStart('/');

        _prototype.Url = $"{gateway}/{path}";
        return this;
    }

    public IApiClientCallBuilder WithServiceName(string serviceName)
    {
        _prototype.ServiceName = serviceName;
        return this;
    }

    public IApiClientCallBuilder WithMethod(HttpMethod method)
    {
        _prototype.Method = method;
        return this;
    }

    public IApiClientCallBuilder WithHeader(string key, string value)
    {
        _prototype.Headers.Add(key, value);
        return this;
    }
    
    public IApiClientCallBuilder WithBody(object body)
    {
        _prototype.Body = body;
        return this;
    }
    
    public IApiClientCallBuilder WithTimeout(CallTimeout timeout)
    {
        _prototype.TimeoutEnum = timeout;
        return this;
    }
    
    public IApiClientCallBuilder WithErrorDto<TError>()
    {
        _prototype.ErrorDtoType = typeof(TError);
        return this;
    }

    public IApiClientCallBuilder WithoutAuthHeader()
    {
        _prototype.SkipAuthHeader = true;
        return this;
    }

    public IApiClientCallBuilder WithAppToken()
    {
        _prototype.UseAppToken = true;
        return this;
    }

    public Task<ApiResponseEnvelope> ExecuteAsync()
    {
        _normalizationService.Apply(_prototype);
        return _call.ExecuteAsync(_prototype);
    }

    public Task<ApiResponseEnvelope> ExecuteSafeAsync()
    {
        _normalizationService.Apply(_prototype);
        return _call.ExecuteSafeAsync(_prototype);
    }
    
}