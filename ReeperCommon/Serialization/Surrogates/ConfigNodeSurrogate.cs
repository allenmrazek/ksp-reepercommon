using System.Reflection;
using System.Runtime.Serialization;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class ConfigNodeSurrogate : ISerializationSurrogate<ConfigNode>
    {
        public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        {
            if (config.HasNode(field.Name))
                throw new SerializationException("Config already has a ConfigNode named " + field.Name);

            if (field.FieldType != typeof (ConfigNode))
                throw new SerializationException("Field is not a ConfigNode");

            config.AddNode((ConfigNode) field.GetValue(fieldOwner));
        }


        public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        {
            if (!config.HasNode(field.Name))
            {
                field.SetValue(fieldOwner, new ConfigNode(field.Name));
                return;
            }

            field.SetValue(fieldOwner, config.GetNode(field.Name));
        }
    }
}
