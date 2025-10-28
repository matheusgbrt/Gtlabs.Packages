﻿using StackExchange.Redis;

namespace Gtlabs.Redis;

internal class RedisConnectionManager : IDisposable
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    private readonly int _defaultDatabase;

    public RedisConnectionManager()
    {
        var conn = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
        var db = Environment.GetEnvironmentVariable("REDIS_DEFAULT_DB");

        _defaultDatabase = int.TryParse(db, out var parsed) ? parsed : 0;

        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(conn)
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