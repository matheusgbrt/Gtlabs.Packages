namespace Gtlabs.Api.ApiCall;

public interface IApiClientCallBuilder
{
    IApiClientCallBuilder WithUrl(string path);
    IApiClientCallBuilder WithServiceName(string serviceName);
    IApiClientCallBuilder WithMethod(HttpMethod method);
    IApiClientCallBuilder WithHeader(string key, string value);
    IApiClientCallBuilder WithBody(object body);
    IApiClientCallBuilder WithTimeout(TimeSpan timeout);
    IApiClientCallBuilder WithErrorDto<TError>();

    Task<ApiResponseEnvelope> ExecuteAsync();
    Task<ApiResponseEnvelope> ExecuteSafeAsync();
}