using System.Reflection;

namespace ReeperCommon.Serialization
{
    public interface ISerializationSurrogate
    {
        void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter);
        void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter);
    }

    public interface ISerializationSurrogate<T> : ISerializationSurrogate
    {
        
    }
}
