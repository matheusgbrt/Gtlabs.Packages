using System.Threading.Tasks;

namespace Gtlabs.Authentication.Services;

public interface IAuthCacheService
{
    Task<string> GetCachedServiceToken(string appId);
}