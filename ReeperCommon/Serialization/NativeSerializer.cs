using System;
using System.Linq;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class NativeSerializer : IConfigNodeItemSerializer
    {
        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(uniqueKey)) throw new ArgumentNullException("uniqueKey");
            if (!type.IsInstanceOfType(target) && target != null) // if target is null, we might not be able to determine its type. But that's okay since nothing will be written
                throw new WrongNativeSerializerException(type, target);
            if (!typeof(IReeperPersistent).IsAssignableFrom(type))
                throw new ArgumentException("Couldn't cast " + type.FullName + " to " + typeof(IReeperPersistent).FullName);


            var reeperPersistent = target as IReeperPersistent;
            if (reeperPersistent == null)
                throw new Exception("Failed to cast target " + type.FullName + " to " +
                                    typeof (IReeperPersistent).FullName);

            // we'll add a brand new node so any keys the target's serialization methods use won't
            // clash with existing keys
            var persistentConfig = config.AddNode(uniqueKey);
            
            if (target is IPersistenceSave)
                (target as IPersistenceSave).PersistenceSave();

            reeperPersistent.DuringSerialize(serializer, persistentConfig);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(uniqueKey)) throw new ArgumentNullException("uniqueKey");
            if (!type.IsInstanceOfType(target) && target != null) // if target is null, we might not be able to determine its type. But that's okay since nothing will be written
                throw new WrongNativeSerializerException(type, target);
            if (!typeof(IReeperPersistent).IsAssignableFrom(type))
                throw new ArgumentException("Couldn't cast " + type.FullName + " to " + typeof(IReeperPersistent).FullName);
            if (!config.HasNode(uniqueKey))
                return target;

            var canCreateDefault = type.GetConstructors().Any(ci => ci.GetParameters().Length == 0 && ci.IsPublic);

            if (!canCreateDefault && target == null)
                throw new NoDefaultValueException(type);

            var reeperPersistent = (target ?? Activator.CreateInstance(type)) as IReeperPersistent;
            if (reeperPersistent == null) // uh ... how??
                throw new Exception("Failed to create instance of type " + type.FullName + " for unknown reasons");

            // new node so any keys the target's serialization methods use won't
            // clash with existing keys
            var configValue = config.GetNode(uniqueKey);

            reeperPersistent.DuringDeserialize(serializer, configValue);

            if (reeperPersistent is IPersistenceLoad)
                (reeperPersistent as IPersistenceLoad).PersistenceLoad();

            return reeperPersistent;
        }
    }
}
