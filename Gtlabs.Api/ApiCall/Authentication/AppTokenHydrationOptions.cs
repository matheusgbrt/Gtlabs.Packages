namespace Gtlabs.Api.ApiCall.Authentication;

public class AppTokenHydrationOptions
{
    public TimeSpan MemoryCacheDuration { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan RedisCacheDuration { get; set; } = TimeSpan.FromMinutes(30);
    public string AuthorizationServiceName { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public HttpMethod TokenRequestMethod { get; set; } = HttpMethod.Post;
    public bool SkipTokenRequestWhenCurrentAppIsAuthorizationService { get; set; } = true;
}
