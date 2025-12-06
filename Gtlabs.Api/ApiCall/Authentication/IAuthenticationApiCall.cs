namespace Gtlabs.Api.ApiCall.Authentication;

public interface IAuthenticationApiCall
{
    Task<string> RequestAppToken();
}