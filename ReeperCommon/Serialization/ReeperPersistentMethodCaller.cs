using System;

namespace ReeperCommon.Serialization
{
    public class ReeperPersistentMethodCaller : IConfigNodeItemSerializer
    {
        public readonly IConfigNodeItemSerializer DecoratedSerializer;

        public ReeperPersistentMethodCaller(IConfigNodeItemSerializer decoratedSerializer)
        {
            if (decoratedSerializer == null) throw new ArgumentNullException("decoratedSerializer");

            DecoratedSerializer = decoratedSerializer;
        }


        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (target == null) return;

            var persistObject = target as IPersistenceSave;

            if (persistObject != null && typeof(IPersistenceSave).IsAssignableFrom(type))
                persistObject.PersistenceSave();

            DecoratedSerializer.Serialize(type, target, uniqueKey, config, serializer);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var result = DecoratedSerializer.Deserialize(type, target, uniqueKey, config, serializer);

            var persistObject = result as IPersistenceLoad;

            if (persistObject != null && typeof(IPersistenceLoad).IsAssignableFrom(type))
                persistObject.PersistenceLoad();

            return result;
        }
    }
}
