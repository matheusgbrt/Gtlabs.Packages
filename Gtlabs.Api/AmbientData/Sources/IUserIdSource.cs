namespace Gtlabs.Api.AmbientData.Sources;

public interface IUserIdSource
{
    Guid? GetUserId();
}