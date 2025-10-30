using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.AmbientData;

public class AmbientData : IAmbientData, IScopedDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AmbientData(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            return null;

        if (context.Request.Headers.TryGetValue("UserID", out var userIdHeader))
            return Guid.Parse(userIdHeader);

        return null;
    }
}