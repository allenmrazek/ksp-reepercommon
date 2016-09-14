namespace ReeperCommon.Repositories
{
    public abstract class Repository<TKey, TValue>
    {
        public abstract bool Has(TKey key);
        public abstract TValue Get(TKey key);
        public abstract void Set(TKey key, TValue value);
        public abstract void Clear();
    }
}