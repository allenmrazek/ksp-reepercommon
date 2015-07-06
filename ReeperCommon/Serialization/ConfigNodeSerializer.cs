using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class ConfigNodeSerializer : IConfigNodeSerializer
    {
        private readonly IGetFieldInfo _serializableGetField;
        private ISerializerSelector _serializerSelector;


        private delegate void FieldAction(object target, ConfigNode config, FieldInfo fieldInfo);

        private delegate void ObjectAction(
            ISerializer objectSerializer, object target, string key, ConfigNode config, IConfigNodeSerializer serializer
            );




        public ConfigNodeSerializer(ISerializerSelector selector, IGetFieldInfo serializableGetField)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (serializableGetField == null) throw new ArgumentNullException("serializableGetField");

            SerializerSelector = selector;
            _serializableGetField = serializableGetField;
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


        public void Deserialize(object target, ConfigNode config)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (config == null) throw new ArgumentNullException("config");

            DoOperation(target, config, DeserializeField,
                (serializer, o, key, node, nodeSerializer) =>
                    serializer.Deserialize(target.GetType(), target, target.GetType().Name, config, nodeSerializer));
        }


        public void Serialize(object source, ConfigNode config)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");

            DoOperation(source, config, SerializeField,
                (serializer, target, key, node, nodeSerializer) =>
                    serializer.Serialize(target.GetType(), target, target.GetType().Name, config, nodeSerializer));
        }


        private void DoOperation(object target, ConfigNode config, FieldAction fieldAction, ObjectAction objectAction)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (config == null) throw new ArgumentNullException("config");
            if (fieldAction == null) throw new ArgumentNullException("fieldAction");
            if (objectAction == null) throw new ArgumentNullException("objectAction");

            var serializableFields = GetSerializableFields(target).ToList();

            serializableFields.ForEach(field => fieldAction(target, config, field));


            // do we have a specialty serializer for this type? if so, we should use it
            var objectSerializer = SerializerSelector.GetSerializer(target.GetType());

            if (objectSerializer.Any())
                objectAction(objectSerializer.Single(), target, target.GetType().Name, config, this);
        }


        private void SerializeField(object sourceField, ConfigNode config, FieldInfo field)
        {
            if (sourceField == null) throw new ArgumentNullException("sourceField");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(sourceField);
            var serializer = SerializerSelector.GetSerializer(field.FieldType);

            if (!serializer.Any())
                throw new NoSerializerFoundException(field.FieldType);

            serializer.Single().Serialize(field.FieldType, fieldInstance, field.Name, config, this);
        }



        private void DeserializeField(object targetField, ConfigNode config, FieldInfo field)
        {
            if (targetField == null) throw new ArgumentNullException("targetField");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(targetField);
            var serializer = SerializerSelector.GetSerializer(field.FieldType);

            if (!serializer.Any())
                throw new NoSerializerFoundException(field.FieldType);

            var result = serializer.Single().Deserialize(field.FieldType, fieldInstance, field.Name, config, this);

            field.SetValue(targetField, result);

            //if (targetField == null) throw new ArgumentNullException("targetField");
            //if (config == null) throw new ArgumentNullException("config");
            //if (field == null) throw new ArgumentNullException("field");

            //var fieldInstance = field.GetValue(targetField);

            ////if (fieldInstance is IReeperPersistent)
            ////    if (!config.HasNode(field.Name))
            ////        throw new SerializationException("Config does not have a node named \"" + field.Name + "\"");
            ////    else
            ////    {
            ////        var subNode = config.GetNode(field.Name);

            ////        Deserialize(fieldInstance, subNode);
            ////        (fieldInstance as IReeperPersistent).Deserialize(this, subNode);
            ////    }
            ////else
            ////{
            //    var serializer = SerializerSelector.GetSerializer(field.FieldType);

            //    if (!serializer.Any())
            //        throw new NoSerializerFoundException(field.FieldType);

            //    var deserializedObject = serializer.Single().Deserialize(fieldInstance, field.Name, config, this);
            //    field.SetValue(targetField, deserializedObject);

            //    //var surrogate = SerializerSelector.GetSurrogate(field.FieldType);

            //    //if (!surrogate.Any())
            //    //    throw new SerializationException("Could not deserialize a field of type " + field.FieldType.FullName + "; no surrogate found");

            //    //surrogate.Single().Deserialize(targetField, field, config, this);
            ////}
        }


        private IEnumerable<FieldInfo> GetSerializableFields(object target)
        {
            return _serializableGetField.Get(target);
        }
    }
}
