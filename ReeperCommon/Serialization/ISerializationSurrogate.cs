using System.Reflection;

namespace ReeperCommon.Serialization
{
    public interface ISerializationSurrogate
    {
        void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter);
        void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter);
    }

    public interface ISerializationSurrogate<T> : ISerializationSurrogate
    {
        
    }
}
