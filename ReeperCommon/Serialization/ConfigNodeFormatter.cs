using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReeperCommon.Serialization
{
    public class ConfigNodeFormatter : IConfigNodeFormatter
    {
        private readonly IFieldInfoQuery _serializableFieldQuery;
        private ISurrogateSelector _surrogateSelector;

        public ConfigNodeFormatter(ISurrogateSelector selector, IFieldInfoQuery serializableFieldQuery)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (serializableFieldQuery == null) throw new ArgumentNullException("serializableFieldQuery");

            SurrogateSelector = selector;
            _serializableFieldQuery = serializableFieldQuery;
        }


        public ISurrogateSelector SurrogateSelector
        {
            get { return _surrogateSelector; }
            set
            {
                if (value == null) throw new ArgumentException("value cannot be null");
                _surrogateSelector = value; 
            }
        }


        public void Deserialize(object target, ConfigNode config)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (config == null) throw new ArgumentNullException("config");

            GetSerializableFields(target).ToList().ForEach(field => DeserializeField(target, config, field));

            if (target is IPersistenceLoad)
                (target as IPersistenceLoad).PersistenceLoad();
        }


        public void Serialize(object source, ConfigNode config)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");

            if (source is IPersistenceSave)
                (source as IPersistenceSave).PersistenceSave();

            var serializableFields = GetSerializableFields(source).ToList();

            if (!serializableFields.Any())
                throw new SerializationException("Did not find any serializable fields in " + source.GetType().FullName);

            serializableFields.ForEach(field =>
            {
                if (config.HasValue(field.Name))
                    throw new SerializationException("ConfigNode already has a value for field \"" + field.Name +
                                                     "\" assigned");

                SerializeField(source, config, field);
            });
        }


        private void SerializeField(object source, ConfigNode config, FieldInfo field)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(source);

            // we might not need a surrogate to serialize this field if it contains appropriate
            // methods
            if (fieldInstance is IReeperPersistent)
            {
                if (config.HasNode(field.Name))
                    throw new SerializationException("Config already has a node named \"" + field.Name + "\"");

                var subNode = config.AddNode(field.Name);

                Serialize(fieldInstance, subNode);
                (fieldInstance as IReeperPersistent).Save(this, subNode);
            }
            else
            {
                // look for a surrogate
                var surrogate = SurrogateSelector.GetSurrogate(field.FieldType);

                if (!surrogate.Any())
                    throw new SerializationException("Could not serialize a field of type " + field.FieldType.FullName);

                surrogate.Single().Serialize(source, field, config, this);
            }
        }



        private void DeserializeField(object target, ConfigNode config, FieldInfo field)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (config == null) throw new ArgumentNullException("config");
            if (field == null) throw new ArgumentNullException("field");

            var fieldInstance = field.GetValue(target);

            if (fieldInstance is IReeperPersistent)
                if (!config.HasNode(field.Name))
                    throw new SerializationException("Config does not have a node named \"" + field.Name + "\"");
                else
                {
                    var subNode = config.GetNode(field.Name);

                    Deserialize(fieldInstance, subNode);
                    (fieldInstance as IReeperPersistent).Load(this, subNode);
                }
            else
            {
                var surrogate = SurrogateSelector.GetSurrogate(field.FieldType);

                if (!surrogate.Any())
                    throw new SerializationException("Could not deserialize a field of type " + field.FieldType.FullName + "; no surrogate found");

                surrogate.Single().Deserialize(target, field, config, this);
            }
        
    }


        private IEnumerable<FieldInfo> GetSerializableFields(object target)
        {
            return _serializableFieldQuery.Get(target);
        }
    }
}
