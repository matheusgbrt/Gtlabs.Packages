namespace Gtlabs.Persistence.DataFilters;

public class DataFilter<TFilter> : IDataFilter<TFilter>
{
    private readonly List<DataFilterState> _states = [];

    public bool IsEnabled => _states.Count == 0 || _states[^1].IsEnabled;

    public IDisposable Enable()
        => SetEnabled(true);

    public IDisposable Disable()
        => SetEnabled(false);

    private IDisposable SetEnabled(bool isEnabled)
    {
        var state = new DataFilterState(isEnabled);
        _states.Add(state);
        return new DisposeAction(() => _states.Remove(state));
    }

    private sealed class DataFilterState
    {
        public DataFilterState(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; }
    }

    private sealed class DisposeAction : IDisposable
    {
        private Action? _action;

        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action?.Invoke();
            _action = null;
        }
    }
}
