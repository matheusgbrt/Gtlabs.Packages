using Gtlabs.Consts.Authentication;

namespace Gtlabs.Authentication.Services;

public interface IJwtEmissorService
{
    string GenerateAppPermissionToken(AppPermission appPermission);
}