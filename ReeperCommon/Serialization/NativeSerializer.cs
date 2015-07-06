using System;
using System.Reflection;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class NativeSerializer : ISerializationNative
    {
        private readonly Type _type;

        public NativeSerializer(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!typeof (IReeperPersistent).IsAssignableFrom(type))
                throw new ArgumentException(type.FullName + " does not implement IReeperPersistent");

            _type = type;
        }


        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(uniqueKey)) throw new ArgumentException("uniqueKey must be provided");
            if (!_type.IsInstanceOfType(target))
                throw new WrongNativeSerializerException(_type, target);
            if (!(target is IReeperPersistent))
                throw new ArgumentException("Couldn't cast " + _type.FullName + " to IReeperPersistent");

            // we'll add a brand new node so any keys the target's serialization methods use won't
            // clash with existing keys
            var persistentConfig = config.AddNode(uniqueKey);
            var reeperPersistent = target as IReeperPersistent;

            if (target is IPersistenceSave)
                (target as IPersistenceSave).PersistenceSave();

            reeperPersistent.Serialize(serializer, persistentConfig);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {

            throw new NotImplementedException();
        }
    }
}
