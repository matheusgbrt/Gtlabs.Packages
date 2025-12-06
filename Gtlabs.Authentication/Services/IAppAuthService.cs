using System.Threading.Tasks;

namespace Gtlabs.Authentication.Services;

public interface IAppAuthService
{
    Task<string> GetAppToken();
    Task SetAppToken(string token);
}