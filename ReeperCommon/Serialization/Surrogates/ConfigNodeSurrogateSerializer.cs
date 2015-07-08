using System;
using System.Linq;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class ConfigNodeSurrogateSerializer : ISurrogateSerializer<ConfigNode>
    {
        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if ((target != null && !type.IsInstanceOfType(target)) || type != typeof (ConfigNode))
                throw new WrongSerializerException(type, typeof (ConfigNode));
            if (target == null) return;

            if (config.HasNode(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            var copy = ((ConfigNode) target).CreateCopy();
            config.AddNode(uniqueKey, copy);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if ((target != null && !type.IsInstanceOfType(target)) || type != typeof(ConfigNode))
                throw new WrongSerializerException(type, typeof(ConfigNode));

            // we don't want to modify any values in the original config so if
            //  we're given the same object as target and input config, create a new config instead
            var deserialized = (ReferenceEquals(target, config) ? null : target) as ConfigNode ?? new ConfigNode(uniqueKey);

            if (!config.HasNode(uniqueKey))
                return deserialized;

            var serializedNodes = config.GetNodes(uniqueKey).ToList();
            if (serializedNodes.Count > 1)
                throw new AmbiguousKeyException(uniqueKey);

            deserialized.ClearData(); 
            serializedNodes.First().CopyTo(deserialized);

            return deserialized;
        }
    }
}
