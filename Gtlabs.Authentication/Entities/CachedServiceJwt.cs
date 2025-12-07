using Gtlabs.Redis.Abstractions;

namespace Gtlabs.Authentication.Entities;

public class CachedServiceJwt : CacheEntity
{
    public CachedServiceJwt(){}
    public CachedServiceJwt(string id)
    {
        Id = id;
    }
    public override string Prefix { get; } = "service-jwt";
    public string Token { get; set; } = string.Empty;
}