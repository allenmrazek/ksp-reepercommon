using System;
using System.Linq;
using System.Runtime.Serialization;
using ReeperCommon.Logging;

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


        public bool Deserialize(object target, ConfigNode config)
        {
            throw new NotImplementedException();
        }


        public bool Serialize(object source, ConfigNode config)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (config == null) throw new ArgumentNullException("config");

            ConfigNode.Merge(config, ConfigNode.CreateConfigFromObject(source));

            var serializableFields = _serializableFieldQuery.Get(source).ToList();

            return serializableFields.All(field => { 
                if (config.HasValue(field.Name))                        
                    throw new SerializationException("ConfigNode already has a value for field \"" + field.Name +
                                                        "\" assigned");

                
            });
            
        }



        
    }
}
