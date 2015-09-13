//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using ReeperCommon.Containers;
//using ReeperCommon.Serialization.Exceptions;

//namespace ReeperCommon.Serialization.Surrogates
//{
//    public class ListSurrogate<TListItemType> : IConfigNodeItemSerializer<IList<TListItemType>>
//    {
//        private const string ListItemNodeName = "item";

//        public void Serialize(
//            Type type, 
//            object target, 
//            string uniqueKey, 
//            ConfigNode config, 
//            IConfigNodeSerializer serializer)
//        {
//            if (type == null) throw new ArgumentNullException("type");
//            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
//            if (config == null) throw new ArgumentNullException("config");
//            if (serializer == null) throw new ArgumentNullException("serializer");
//            if (string.IsNullOrEmpty(uniqueKey))
//                throw new ArgumentException("Invalid key: " + uniqueKey, "uniqueKey");

//            if (!typeof (List<TListItemType>).IsAssignableFrom(type))
//                throw new WrongSerializerException(type, typeof (List<TListItemType>));

//            if (config.HasNode(uniqueKey))
//                throw new ConfigNodeDuplicateKeyException(uniqueKey);

//            var itemSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(typeof(TListItemType));
//            if (!itemSerializer.Any() && type.IsValueType)
//                throw new NoSerializerFoundException(type);

//            var node = config.AddNode(uniqueKey);
//            var list = target as List<TListItemType>;

//            if (list == null)
//                return;

//            foreach (var item in list)
//            {
//                var itemConfig = new ConfigNode(typeof(TListItemType).Name);
//                TListItemType item1 = item;

//                // use surrogate if available
//                //itemSerializer.Do(
//                //    s => s.Serialize(typeof (TListItemType), item1, typeof (TListItemType).Name, itemConfig, serializer));

//                //// otherwise a general serialize will work if it's a reference type
//                //itemSerializer.SingleOrDefault().IfNull(() => serializer.Serialize(item1, itemConfig));
//                serializer.Serialize(item1, itemConfig);
//                node.AddNode(itemConfig);

//                //var itemConfig = serializer.CreateConfigNodeFromObject(item);

//                //itemConfig.Do(cfg =>
//                //{
//                //    itemConfig.name = ListItemNodeName;
//                //    node.AddNode(itemConfig);
//                //});
//            }
//        }


//        public object Deserialize(
//            Type type, 
//            object target, 
//            string uniqueKey, 
//            ConfigNode config, 
//            IConfigNodeSerializer serializer)
//        {
//            if (type == null) throw new ArgumentNullException("type");
//            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
//            if (config == null) throw new ArgumentNullException("config");
//            if (serializer == null) throw new ArgumentNullException("serializer");
//            if (string.IsNullOrEmpty(uniqueKey))
//                throw new ArgumentException("Invalid key: " + uniqueKey, "uniqueKey");

//            if (!typeof(List<TListItemType>).IsAssignableFrom(type))
//                throw new WrongSerializerException(type, typeof(List<TListItemType>));

//            var itemSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(typeof(TListItemType));
//            if (!itemSerializer.Any() && type.IsValueType)
//                throw new NoSerializerFoundException(type);

//            var list = target as List<TListItemType>;


//            list = list ?? Activator.CreateInstance(type) as List<TListItemType>;
//            if (list == null) return null; // we failed to create list somehow...

//            list.Do(l =>
//            {
//                l.Clear();

//                config
//                    .With(cfg => cfg.GetNode(uniqueKey))
//                    .With(c => c.GetNodes(typeof (TListItemType).Name))
//                    .ToList()
//                    .ForEach(itemNode =>
//                    {
//                        var item = CreateDefaultListItem();

//                        serializer.Deserialize(item, itemNode);
//                        l.Add(item);

////                        if (itemSerializer.Any())
////                        {
////                            item = (TListItemType)
////                                itemSerializer.Return(
////                                    s =>
////                                        s.Deserialize(typeof (TListItemType), default(TListItemType),
////                                            typeof (TListItemType).Name,
////                                            itemNode, serializer), default(TListItemType));
////                            l.Add(item);
////                        }
////                        else
////                        {
////// ReSharper disable once CompareNonConstrainedGenericWithNull
////                            if (typeof (TListItemType).IsValueType || (item != null))
////                                serializer.Deserialize(item, itemNode);
////                            l.Add(item);
////                        }
//                    });
//            });

//            return list;
//        }


//        private static TListItemType CreateDefaultListItem()
//        {
//            var tlt = typeof (TListItemType);

//            if (tlt.IsValueType)
//                return default(TListItemType);

//            if (typeof (string) == tlt)
//                return (TListItemType)(object)string.Empty; // no default constructor for string

//            if (!tlt.IsAbstract && tlt.GetConstructors().Any(c => c.GetParameters().Length == 0))
//                return Activator.CreateInstance<TListItemType>();

//            throw new ArgumentException("No suitable default constructor for " + tlt.Name);
//        }
//    }
//}
