using Gtlabs.Redis.Abstractions;

namespace Gtlabs.Authentication.Entities;

public class CachedServiceJwt : CacheEntity
{
    public CachedServiceJwt(string appId)
    {
        Id = appId;
    }
    public override string Prefix { get; } = "service-jwt";
    public string Token { get; set; } = string.Empty;
}