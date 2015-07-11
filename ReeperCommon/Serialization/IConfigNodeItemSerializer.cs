using System;

namespace ReeperCommon.Serialization
{
    public interface IConfigNodeItemSerializer
    {
        void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer);
        object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer);
    }


    public interface IConfigNodeItemSerializer<T> : IConfigNodeItemSerializer
    {
        
    }
}
