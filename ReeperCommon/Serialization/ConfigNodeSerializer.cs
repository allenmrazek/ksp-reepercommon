using System;
using System.Collections.Generic;
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
            //if (target == null) throw new ArgumentNullException("target");
            //if (config == null) throw new ArgumentNullException("config");

            //GetSerializableFields(target).ToList().ForEach(field => DeserializeField(target, config, field));

            //if (target is IPersistenceLoad)
            //    (target as IPersistenceLoad).PersistenceLoad();

            throw new NotImplementedException();
        }


        public void Serialize(object source, ConfigNode config)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");


            // we'll also serialize its fields anyway. Presumably the coder has explicitly
            // marked them to be serialized so it'll be expected anyway
            var serializableFields = GetSerializableFields(source).ToList();

            serializableFields.ForEach(field =>
            {
                if (config.HasValue(field.Name))
                    throw new SerializationException("ConfigNode already has a value for field \"" + field.Name +
                                                     "\" assigned");

                SerializeField(source, config, field);
            });


            // do we have a specialty serializer for this type? if so, we should use it
            var objectSerializer = SerializerSelector.GetSerializer(source.GetType());

            if (objectSerializer.Any())
                objectSerializer.Single().Serialize(source, source.GetType().Name, config, this);
        }
    


        private void SerializeField(object sourceField, ConfigNode config, FieldInfo field)
        {
            if (sourceField == null) throw new ArgumentNullException("sourceField");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(sourceField);

            // we might not need a surrogate to serialize this field if it contains appropriate
            // methods
            //if (fieldInstance is IReeperPersistent)
            //{
            //    if (config.HasNode(field.Name))
            //        throw new SerializationException("Config already has a node named \"" + field.Name + "\"");

            //    var subNode = config.AddNode(field.Name);

            //    Serialize(fieldInstance, subNode);
            //    (fieldInstance as IReeperPersistent).Serialize(this, subNode);
            //}
            //else
            //{

            var serializer = SerializerSelector.GetSerializer(field.FieldType);

            if (!serializer.Any())
                throw new NoSerializerFoundException(field.FieldType);

            serializer.Single().Serialize(fieldInstance, field.Name, config, this);

            //}
        }



        private void DeserializeField(object targetField, ConfigNode config, FieldInfo field)
        {

            throw new NotImplementedException();

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
