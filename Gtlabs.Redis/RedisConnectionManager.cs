using Gtlabs.Redis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Gtlabs.Redis;

internal class RedisConnectionManager : IDisposable
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    private readonly int _defaultDatabase;

    public RedisConnectionManager(IOptions<RedisOptions> options)
    {
        var settings = options.Value;
        _defaultDatabase = settings.DefaultDb;

        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(settings.Connection)
        );
    }

    public ConnectionMultiplexer Connection => _lazyConnection.Value;

    public IDatabase GetDatabase() => Connection.GetDatabase(_defaultDatabase);

    public void Dispose()
    {
        if (_lazyConnection.IsValueCreated)
            Connection.Dispose();
    }
}