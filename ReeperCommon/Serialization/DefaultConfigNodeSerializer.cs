using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public class DefaultConfigNodeSerializer : IConfigNodeSerializer
    {
        //public DefaultConfigNodeSerializer(IEnumerable<Assembly> assembliesToSearchForSurrogates) 
        //    : base(
        //        new SerializerSelectorDecorator(
        //            new PreferNativeSerializer(
        //                new SerializerSelector(
        //                    new SurrogateProvider(
        //                        new GetSerializationSurrogates(
        //                            new GetSurrogateSupportedTypes()
        //                        ), 
        //                        new GetSurrogateSupportedTypes(), 
        //                        assembliesToSearchForSurrogates
        //                    ))
        //                )
        //            ,
        //            s => Maybe<IConfigNodeItemSerializer>.With(new FieldSerializer(s, new GetSerializableFields())
        //        )))
        //{
           
        //}

        public DefaultConfigNodeSerializer(IEnumerable<Assembly> assembliesToScanForSurrogates)
        {
            if (assembliesToScanForSurrogates == null) throw new ArgumentNullException("assembliesToScanForSurrogates");

            var supportedTypeQuery = new GetSurrogateSupportedTypes();
            var surrogateQuery = new GetSerializationSurrogates(supportedTypeQuery);
            var serializableFieldQuery = new GetSerializableFields();

            var standardSerializerSelector =
                new SerializerSelector(new SurrogateProvider(surrogateQuery, supportedTypeQuery,
                    assembliesToScanForSurrogates));

            //standardSerializerSelector.
            throw new NotImplementedException();
        }
        public ConfigNode CreateConfigNodeFromObject(object target)
        {
            throw new NotImplementedException();
        }

        public void WriteObjectToConfigNode(ref object source, ConfigNode config)
        {
            throw new NotImplementedException();
        }

        public void WriteObjectToConfigNode<T>(ref T source, ConfigNode config)
        {
            throw new NotImplementedException();
        }

        public void LoadObjectFromConfigNode<T>(ref T target, ConfigNode config)
        {
            throw new NotImplementedException();
        }

        public ISerializerSelector SerializerSelector { get; set; }
    }
}
