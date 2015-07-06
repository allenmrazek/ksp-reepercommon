using System;

namespace ReeperCommon.Serialization
{
    public interface ISerializer
    {
        // These for serializing fields, necessary because we have to write value types back into the field
        // after 
        //void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer serializer);
        //void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer serializer);

        // These used to serialize a particular item into supplied config
        void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer);
        object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer);
    }
}
