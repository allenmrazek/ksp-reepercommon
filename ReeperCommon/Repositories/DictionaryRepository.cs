using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ReeperCommon.Logging;

namespace ReeperCommon.Repositories
{
    // ReSharper disable once UnusedMember.Global
    public class DictionaryRepository<TKey, TValue> : Repository<TKey, TValue>
    {
        protected readonly Func<TKey, TValue> Factory;
        protected readonly Dictionary<TKey, TValue> Cache = new Dictionary<TKey, TValue>();

        public DictionaryRepository([NotNull] Func<TKey, TValue> factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            Factory = factory;
        }

        public DictionaryRepository()
        {
            
        }


        public override bool Has(TKey key)
        {
            Log.Warning("Checking has " + key + " : " + Cache.ContainsKey(key));
            return Cache.ContainsKey(key);
        }


        public override TValue Get(TKey key)
        {
            TValue value;

            if (Cache.TryGetValue(key, out value)) return value;

            value = Factory != null ? Factory(key) : Create(key);
            Cache.Add(key, value);
            
            return value;
        }


        public override void Set(TKey key, TValue value)
        {
            Cache[key] = value;
            Log.Warning("Setting value " + value + " for key " + key);
        }

        public override void Clear()
        {
            Cache.Clear();
        }


        protected virtual TValue Create(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
