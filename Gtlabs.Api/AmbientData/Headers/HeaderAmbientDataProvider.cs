using Gtlabs.Api.AmbientData.Sources;
using Microsoft.AspNetCore.Http;

namespace Gtlabs.Api.AmbientData.Headers;

public class HeaderAmbientDataProvider : IUserIdSource, IOrderedAmbientSource
{

    public int Order { get; } = 0;
    private readonly IHttpContextAccessor _accessor;

    public HeaderAmbientDataProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    public Guid? GetUserId()
    {
        var ctx = _accessor.HttpContext;
        if (ctx?.Request.Headers.TryGetValue("UserID", out var h) == true)
            return Guid.Parse(h);
        return null;
    }

}