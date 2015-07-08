using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
    public class PrimitiveSurrogateSerializer : 
        ISurrogateSerializer<string>,
        ISurrogateSerializer<bool>,
        ISurrogateSerializer<int>,
        ISurrogateSerializer<float>,
        ISurrogateSerializer<double>
    {
        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (target == null)
                return; // don't serialize nulls
            if (string.IsNullOrEmpty(uniqueKey)) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");

            CheckSupportedTypes(type);

            if (config.HasValue(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            var tc = TypeDescriptor.GetConverter(type);

            if (!tc.CanConvertTo(typeof(string)))
                throw new NoConversionException(type, typeof (string));

            if (!tc.IsValid(target))
                throw new InvalidDataException("target data is invalid for " + type.FullName + " TypeConverter");

            var strValue = tc.ConvertToInvariantString(target);

            config.AddValue(uniqueKey, strValue);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");

            if (string.IsNullOrEmpty(uniqueKey))
                throw new ArgumentNullException("uniqueKey");

            CheckSupportedTypes(type);

            if (!config.HasValue(uniqueKey))
                return target; // no changes

            var tc = TypeDescriptor.GetConverter(type);

            if (!tc.CanConvertFrom(typeof(string)))
                throw new NoConversionException(typeof(string), type);

            var strValue = config.GetValue(uniqueKey);

            return tc.ConvertFromInvariantString(strValue);
        }


        public IEnumerable<Type> GetSupportedTypes()
        {
            return GetType()
                .GetInterfaces()
                .SelectMany(i => i.GetGenericArguments());
        }


        private void CheckSupportedTypes(Type targetType)
        {
            if (GetSupportedTypes().All(t => t != targetType))
                throw new NotSupportedException(targetType.FullName + " is not supported by this surrogateSerializer. It handles " + string.Join(",", GetSupportedTypes().Select(t => t.FullName).ToArray()));
        }
    }
}
