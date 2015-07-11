﻿using System;
using System.Reflection;
using System.Runtime.Serialization;
using ReeperCommon.Extensions;
using ReeperCommon.Serialization.Exceptions;
using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class RectSurrogateSerializer : IConfigNodeItemSerializer<Rect>
    {
        //public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (config.HasNode(field.Name))
        //        throw new SerializationException("A node named " + field.Name + " has already been defined");

        //    var cfg = config.AddNode(field.Name);
        //    var r = (Rect)field.GetValue(fieldOwner);

        //    cfg.AddValue("x", r.x);
        //    cfg.AddValue("y", r.y);
        //    cfg.AddValue("width", r.width);
        //    cfg.AddValue("height", r.height);
        //}


        //public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
        //{
        //    if (!config.HasNode(field.Name))
        //        return; // don't change existing value 

        //    var rectConfig = config.GetNode(field.Name);
        //    var r = (Rect)field.GetValue(fieldOwner);

        //    r.x = rectConfig.Parse("x", r.x);
        //    r.y = rectConfig.Parse("y", r.y);
        //    r.width = rectConfig.Parse("width", r.width);
        //    r.height = rectConfig.Parse("height", r.height);

        //    field.SetValue(fieldOwner, r);
        //}


        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!(target is Rect)) throw new WrongSerializerException(type, typeof (Rect));

            if (config.HasNode(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            var cfg = config.AddNode(uniqueKey);
            var r = (Rect) target;

            cfg.AddValue("x", r.x);
            cfg.AddValue("y", r.y);
            cfg.AddValue("width", r.width);
            cfg.AddValue("height", r.height);
        }

        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!(target is Rect)) throw new WrongSerializerException(type, typeof(Rect));

            if (!config.HasNode(uniqueKey))
                return target; // no changes; leave existing values intact


            var rectConfig = config.GetNode(uniqueKey);
            var r = (Rect) target;

            r.x = rectConfig.Parse("x", r.x);
            r.y = rectConfig.Parse("y", r.y);
            r.width = rectConfig.Parse("width", r.width);
            r.height = rectConfig.Parse("height", r.height);

            return r;
        }
    }
}
