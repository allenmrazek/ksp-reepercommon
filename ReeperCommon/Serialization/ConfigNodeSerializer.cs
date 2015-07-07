using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
    public class ConfigNodeSerializer : IConfigNodeSerializer
    {
        private readonly IGetObjectFields _serializableGetField;
        private IConfigNodeItemSerializerSelector _configNodeItemSerializerSelector;


        private delegate void FieldAction(object target, ConfigNode config, FieldInfo fieldInfo);

        private delegate void ObjectAction(
            IConfigNodeItemSerializer objectConfigNodeItemSerializer, object target, string key, ConfigNode config, IConfigNodeSerializer serializer
            );




        public ConfigNodeSerializer(IConfigNodeItemSerializerSelector selector, IGetObjectFields serializableGetField)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (serializableGetField == null) throw new ArgumentNullException("serializableGetField");

            ConfigNodeItemSerializerSelector = selector;
            _serializableGetField = serializableGetField;
        }


        public IConfigNodeItemSerializerSelector ConfigNodeItemSerializerSelector
        {
            get { return _configNodeItemSerializerSelector; }
            set
            {
                if (value == null) throw new ArgumentException("value cannot be null");
                _configNodeItemSerializerSelector = value;
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
            var objectSerializer = ConfigNodeItemSerializerSelector.GetSerializer(target.GetType());

            if (objectSerializer.Any())
                objectAction(objectSerializer.Single(), target, target.GetType().Name, config, this);
        }


        private void SerializeField(object sourceField, ConfigNode config, FieldInfo field)
        {
            if (sourceField == null) throw new ArgumentNullException("sourceField");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(sourceField);
            var serializer = ConfigNodeItemSerializerSelector.GetSerializer(field.FieldType);

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
            var serializer = ConfigNodeItemSerializerSelector.GetSerializer(field.FieldType);

            if (!serializer.Any())
                throw new NoSerializerFoundException(field.FieldType);

            var result = serializer.Single().Deserialize(field.FieldType, fieldInstance, field.Name, config, this);

            field.SetValue(targetField, result);
        }


        private IEnumerable<FieldInfo> GetSerializableFields(object target)
        {
            return _serializableGetField.Get(target);
        }
    }
}
