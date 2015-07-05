using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
    public class PrimitiveSurrogate : 
        ISerializationSurrogate<string>,
        ISerializationSurrogate<bool>,
        ISerializationSurrogate<int>,
        ISerializationSurrogate<float>,
        ISerializationSurrogate<double>
    {
        public void Serialize(object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config.HasValue(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            if (GetSupportedTypes().All(t => t != target.GetType()))
                throw new NotSupportedException(target.GetType().FullName + " is not supported by this surrogate. It handles " + string.Join(",", GetSupportedTypes().Select(t => t.FullName).ToArray()));

            var tc = TypeDescriptor.GetConverter(target.GetType());

            if (!tc.CanConvertTo(typeof(string)))
                throw new NoConversionException(target.GetType(), typeof (string));

            if (!tc.IsValid(target))
                throw new InvalidDataException("target data is invalid for " + target.GetType().FullName + " TypeConverter");

            var strValue = tc.ConvertToInvariantString(target);

            config.AddValue(uniqueKey, strValue);
        }


        public object Deserialize(object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Type> GetSupportedTypes()
        {
            return GetType()
                .GetInterfaces()
                .SelectMany(i => i.GetGenericArguments());
        }
    }
}
