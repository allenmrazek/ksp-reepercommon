using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
    public class ListSurrogate<TListItemType> : IConfigNodeItemSerializer<IList<TListItemType>>
    {
        public void Serialize(
            Type type, 
            object target, 
            string uniqueKey, 
            ConfigNode config, 
            IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(uniqueKey))
                throw new ArgumentException("Invalid key: " + uniqueKey, "uniqueKey");

            if (!typeof (List<TListItemType>).IsAssignableFrom(type))
                throw new WrongSerializerException(type, typeof (List<TListItemType>));

            if (config.HasNode(uniqueKey))
                throw new ConfigNodeDuplicateKeyException(uniqueKey);

            var itemSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(typeof (TListItemType));
            if (!itemSerializer.Any())
                throw new NoSerializerFoundException(typeof (TListItemType));

            var node = config.AddNode(uniqueKey);
            var list = target as List<TListItemType>;

            if (list == null)
                return;

            foreach (var item in list)
            {
                var listItem = item;

                itemSerializer.Do(
                    s =>
                        s.Serialize(typeof (TListItemType), listItem, listItem.GetType().Name, node.AddNode("item"), serializer));
            }
        }

        public object Deserialize(
            Type type, 
            object target, 
            string uniqueKey, 
            ConfigNode config, 
            IConfigNodeSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
