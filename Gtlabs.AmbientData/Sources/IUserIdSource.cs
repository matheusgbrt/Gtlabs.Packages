namespace Gtlabs.AmbientData.Sources;

public interface IUserIdSource
{
    Guid? GetUserId();
}