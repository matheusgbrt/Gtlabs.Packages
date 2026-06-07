namespace Gtlabs.Persistence.DataFilters;

public interface IDataFilter<TFilter>
{
    bool IsEnabled { get; }
    IDisposable Enable();
    IDisposable Disable();
}
