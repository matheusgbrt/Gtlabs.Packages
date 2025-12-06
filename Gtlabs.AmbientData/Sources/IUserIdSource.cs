namespace Gtlabs.Core.AmbientData.Sources;

public interface IUserIdSource
{
    Guid? GetUserId();
}