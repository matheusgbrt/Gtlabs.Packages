namespace Gtlabs.Api.ApiCall;

public interface IApiClientCall
{
    Task<ApiResponseEnvelope> ExecuteAsync(ApiClientCallPrototype prototype);
    Task<ApiResponseEnvelope> ExecuteSafeAsync(ApiClientCallPrototype prototype);
}