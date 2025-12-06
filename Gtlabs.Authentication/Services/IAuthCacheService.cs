using System.Threading.Tasks;
using Gtlabs.Authentication.Entities;

namespace Gtlabs.Authentication.Services;

public interface IAuthCacheService
{
    Task<string> GetCachedServiceToken(string appId);
    Task SetCachedServiceToken(CachedServiceJwt cachedServiceJwt);
}