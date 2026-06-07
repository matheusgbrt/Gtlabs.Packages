namespace Gtlabs.Api.ApiCall.Authentication;

public interface IAppTokenProvider
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
}
