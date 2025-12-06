namespace Gtlabs.Api.ApiCall.Authentication;

public interface ITokenProvider
{
    Task<string> GetOrRefreshTokenAsync();
}