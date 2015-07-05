using System;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public abstract class FieldSurrogateToSingleValueBase<T> : ISerializationSurrogate<T>
    {
        //public virtual void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (config.HasValue(field.Name))
        //        throw new SerializationException("Could not deserialize type " + field.FieldType.FullName +
        //                                         " from config because a value named " + field.Name + " already exists");

        //    if (field.FieldType != typeof(T))
        //        throw new InvalidOperationException("Incompatible types: field type " + field.FieldType.FullName +
        //                                            " is not " + typeof (T).FullName);

        //    config.AddValue(field.Name, GetFieldContentsAsString((T)field.GetValue(fieldOwner)));
        //}

        //public virtual void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (!config.HasValue(field.Name))
        //        throw new SerializationException("Could not deserialize type " + field.FieldType.FullName +
        //                                         " from config because no value named " + field.Name + " was found");

        //    if (field.FieldType != typeof(T))
        //        throw new InvalidOperationException("Incompatible types: field type " + field.FieldType.FullName +
        //                                            " is not " + typeof(T).FullName);

        //    field.SetValue(fieldOwner, GetFieldContentsFromString(config.GetValue(field.Name)));
        //}


        protected virtual string GetFieldContentsAsString(T instance)
        {
            return instance.ToString();
        }


        protected abstract T GetFieldContentsFromString(string value);


        public void Serialize(object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config.HasValue(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            if (target.GetType() != typeof (T))
                throw new WrongSerializerException(target.GetType(), typeof (T));

            config.AddValue(uniqueKey, GetFieldContentsAsString((T)target));
        }


        public object Deserialize(object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
