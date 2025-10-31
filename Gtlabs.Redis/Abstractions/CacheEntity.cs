namespace Gtlabs.Redis.Abstractions
{
    public abstract class CacheEntity
    {
        public abstract string Prefix { get; }

        public Guid Id { get; init; }

        public virtual string BuildKey()
        {
            return $"{Prefix}:{Id.ToString()}";
        }
    }
}