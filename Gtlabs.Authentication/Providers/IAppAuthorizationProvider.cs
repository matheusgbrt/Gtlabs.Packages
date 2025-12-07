
using Gtlabs.Api.ApiCall;

namespace Gtlabs.Authentication.Providers;

public interface IAppAuthorizationProvider
{
    Task<ApiResponseEnvelope> GetAppPermissionAsync(string appIdentifier);
}