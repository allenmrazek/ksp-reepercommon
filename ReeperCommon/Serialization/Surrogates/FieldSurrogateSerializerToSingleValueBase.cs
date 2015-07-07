using System;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
    public abstract class FieldSurrogateSerializerToSingleValueBase<T> : ISurrogateSerializer<T>
    {
        protected virtual string GetFieldContentsAsString(T instance)
        {
            return instance.ToString();
        }


        protected abstract T GetFieldContentsFromString(string value);


        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (config.HasValue(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            if ((target != null ? target.GetType() : type) != typeof(T))
                throw new WrongSerializerException(type, typeof(T));

            config.AddValue(uniqueKey, GetFieldContentsAsString((T)target));
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!config.HasValue(uniqueKey))
                return target;

            var strValue = config.GetValue(uniqueKey);

            return GetFieldContentsFromString(strValue);
        }
    }
}
