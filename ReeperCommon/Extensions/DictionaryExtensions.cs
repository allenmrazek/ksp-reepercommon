using System;
using System.Collections.Generic;

namespace ReeperCommon.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
                this IDictionary<TKey, TValue> @this,
                TKey key,
                Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (!@this.TryGetValue(key, out value))
            {
                @this.Add(key, value = valueFactory(key));
            }
            return value;
        }

    }
}
