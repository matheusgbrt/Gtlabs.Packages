using Gtlabs.AmbientData.Interfaces;
using Gtlabs.AmbientData.Sources;
using Gtlabs.Consts;
using Microsoft.AspNetCore.Http;

namespace Gtlabs.AmbientData.Providers;

public class HeaderAmbientDataProvider : IAmbientDataProvider, IUserIdSource, ICorrelationIdSource, IOrderedAmbientSource
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
        if (ctx?.Request.Headers.TryGetValue(HeaderFields.UserId, out var h) == true)
            return Guid.Parse(h);
        return null;
    }

    public Guid? GetCorrelationid()
    {
        var ctx = _accessor.HttpContext;
        if (ctx?.Request.Headers.TryGetValue(HeaderFields.CorrelationId, out var h) == true)
            return Guid.Parse(h);
        return null;
    }
}

