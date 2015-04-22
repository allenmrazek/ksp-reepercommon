using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReeperCommon.Serialization.Surrogates
{
    public abstract class FieldSurrogateBaseToString : ISerializationSurrogate
    {
        public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter)
        {
            if (config.HasValue(field.Name))
                throw new SerializationException("Config already has a field named " + field.Name);

            config.AddValue(field.Name, field.GetValue(fieldOwner).ToString());
        }

        public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter)
        {
            if (!config.HasValue(field.Name))
                throw new SerializationException("Could not find a field named " + field.Name);

            field.SetValue(fieldOwner, config.GetValue(field.Name));
        }
    }
}
