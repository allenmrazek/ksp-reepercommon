using System;
using System.Linq;
using System.Runtime.Serialization;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class NativeSerializer : IConfigNodeItemSerializer
    {
        public const string NativeNodeName = "NativeData";

        public void Serialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!type.IsInstanceOfType(target) && target != null) // if target is null, we might not be able to determine its type. But that's okay since nothing will be written
                throw new WrongNativeSerializerException(type, target);
            if (!typeof(IReeperPersistent).IsAssignableFrom(type))
                throw new ArgumentException("Couldn't cast " + type.FullName + " to " + typeof(IReeperPersistent).FullName);
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Can't be null or empty");

            var reeperPersistent = target as IReeperPersistent;
            if (reeperPersistent == null)
                throw new Exception("Failed to cast target " + type.FullName + " to " +
                                    typeof(IReeperPersistent).FullName);

            // we'll add a brand new node so any keys the target's serialization methods use won't
            // clash with existing keys
            var persistentConfig = config.AddNode(NativeNodeName);

            reeperPersistent.DuringSerialize(serializer, persistentConfig);
        }


        public void Deserialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!type.IsInstanceOfType(target) && target != null) // if target is null, we might not be able to determine its type. But that's okay since nothing will be written
                throw new WrongNativeSerializerException(type, target);
            if (!typeof(IReeperPersistent).IsAssignableFrom(type))
                throw new ArgumentException("Couldn't cast " + type.FullName + " to " + typeof(IReeperPersistent).FullName);
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Can't be null or empty");

            var canCreateDefault = type.GetConstructors().Any(ci => ci.GetParameters().Length == 0 && ci.IsPublic);

            if (!canCreateDefault && target == null)
                throw new NoDefaultValueException(type);

            if (!config.HasNode(NativeNodeName))
                throw new ReeperSerializationException("Can't deserialize " + type.FullName + " because given ConfigNode is missing " + NativeNodeName + " node");


            var reeperPersistent = (target ?? Activator.CreateInstance(type)) as IReeperPersistent;
            if (reeperPersistent == null) // uh ... how??
                throw new Exception("Failed to create instance of type " + type.FullName + " for unknown reasons");

            // new node so any keys the target's serialization methods use won't
            // clash with existing keys
            var configValue = config.GetNode(NativeNodeName);

            reeperPersistent.DuringDeserialize(serializer, configValue);

            target = reeperPersistent;
        }
    }
}
