using System;
using System.Linq;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class ConfigNodeSerializer : IConfigNodeSerializer
    {
        private ISerializerSelector _serializerSelector;


        public ConfigNodeSerializer(ISerializerSelector selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            SerializerSelector = selector;
        }


        public ISerializerSelector SerializerSelector
        {
            get { return _serializerSelector; }
            set
            {
                if (value == null) throw new ArgumentException("value cannot be null");
                _serializerSelector = value;
            }
        }


        public ConfigNode CreateConfigNodeFromObject(object target)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (target == null) throw new ArgumentNullException("target");

            var config = new ConfigNode(target.GetType().FullName);

            WriteObjectToConfigNode(ref target, config);

            return config;
        }


        public void WriteObjectToConfigNode(ref object source, ConfigNode config)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");

            GetSerializer(source.GetType())
                .Serialize(source.GetType(), ref source, new ConfigNode(source.GetType().FullName), this);
        }
 

        public void LoadObjectFromConfigNode(ref object target, ConfigNode config)
        {
            if (target == null) throw new ArgumentNullException("target");

            GetSerializer(target.GetType()).Deserialize(target.GetType(), ref target, config, this);
        }


        private IConfigNodeItemSerializer GetSerializer(Type type)
        {
            var serializer = SerializerSelector.GetSerializer(type);

            if (!serializer.Any())
                throw new NoSerializerFoundException(type);

            return serializer.Single();
        }


        public override string ToString()
        {
            return "ConfigNodeSerializer: " + SerializerSelector;
        }
    }
}
