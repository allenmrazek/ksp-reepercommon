using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class ConfigNodeSurrogateSerializer : ISurrogateSerializer<ConfigNode>
    {
        //public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (config.HasNode(field.Name))
        //        throw new SerializationException("Config already has a ConfigNode named " + field.Name);

        //    if (field.FieldType != typeof (ConfigNode))
        //        throw new SerializationException("Field is not a ConfigNode");

        //    config.AddNode((ConfigNode) field.GetValue(fieldOwner));
        //}


        //public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (!config.HasNode(field.Name))
        //    {
        //        field.SetValue(fieldOwner, new ConfigNode(field.Name));
        //        return;
        //    }

        //    field.SetValue(fieldOwner, config.GetNode(field.Name));
        //}


        //public void SerializeToValue(object target, ConfigNode config, IConfigNodeSerializer serializer)
        //{
        //    if (target == null) throw new ArgumentNullException("target");
        //    if (config == null) throw new ArgumentNullException("config");
        //    if (serializer == null) throw new ArgumentNullException("serializer");

        //    var targetNode = target as ConfigNode;
        //    if (targetNode == null)
        //        throw new WrongSerializerException(target.GetType(), typeof (ConfigNode));


        //}



        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
