using System.Reflection;
using System.Runtime.Serialization;
using ReeperCommon.Extensions;
using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class RectSurrogate : ISerializationSurrogate<Rect>
    {
        public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter)
        {
            if (config.HasNode(field.Name))
                throw new SerializationException("A node named " + field.Name + " has already been defined");

            var cfg = config.AddNode(field.Name);
            var r = (Rect)field.GetValue(fieldOwner);

            cfg.AddValue("x", r.x);
            cfg.AddValue("y", r.y);
            cfg.AddValue("width", r.width);
            cfg.AddValue("height", r.height);
        }


        public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeFormatter formatter)
        {
            if (!config.HasNode(field.Name))
                return; // don't change existing value 

            var rectConfig = config.GetNode(field.Name);
            var r = (Rect)field.GetValue(fieldOwner);

            r.x = rectConfig.Parse("x", r.x);
            r.y = rectConfig.Parse("y", r.y);
            r.width = rectConfig.Parse("width", r.width);
            r.height = rectConfig.Parse("height", r.height);

            field.SetValue(fieldOwner, r);
        }
    }
}
