namespace Gtlabs.Redis.Abstractions
{
    public abstract class CacheEntity
    {
        public abstract string Prefix { get; }

        public string Id { get; set; } = "";

        public virtual string BuildKey()
        {
            return $"{Prefix}:{Id}";
        }
    }
}