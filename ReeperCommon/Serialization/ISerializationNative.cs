using System.Reflection;

namespace ReeperCommon.Serialization
{
    // This serializer is used for types that supply their own internal serialization routines
    // (implement IReeperPersistent, in other words)
    public interface ISerializationNative
    {
        void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter);
        void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter);
    }
}
