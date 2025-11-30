namespace Gtlabs.Redis.Options;

public class RedisOptions
{
    public string Connection { get; set; }
    public int DefaultDb { get; set; } = 0;
}