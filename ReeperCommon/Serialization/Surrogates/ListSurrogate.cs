using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ReeperCommon.Containers;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization.Surrogates
{
    /// <summary>
    /// Note to self: ListSurrogate's item type MUST be serializable so we'll be using the item serializer selector.
    /// This is because any item that doesn't have a serializer (such as a basic object with [ReeperPersistent] fields)
    /// won't be fully initializable... a problem when we'll be creating them
    /// </summary>
    /// <typeparam name="TListItemType"></typeparam>
    public class ListSurrogate<TListItemType> : IConfigNodeItemSerializer<List<TListItemType>>
    {
        private const string ListItemNodeName = "item";

        public void Serialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (key == null) throw new ArgumentNullException("key");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", "key");

            if (!typeof(List<TListItemType>).IsAssignableFrom(type))
                throw new WrongSerializerException(target != null ? target.GetType() : type, typeof(List<TListItemType>));

            if (config.HasNode(key))
                throw new ConfigNodeDuplicateKeyException(key);

            var list = target as List<TListItemType>;
            if (list == null)
                if (target == null)
                    return;
                else throw new WrongSerializerException(target.GetType(), typeof (List<TListItemType>));

            var itemSerializer = serializer.SerializerSelector.GetSerializer(typeof (TListItemType));

            if (!itemSerializer.Any())
                throw new NoSerializerFoundException(typeof(TListItemType));

            foreach (var item in list)
            {
                var objItem = (object) item;

                itemSerializer.Single().Serialize(typeof(TListItemType), ref objItem, typeof(TListItemType).FullName, config.AddNode(ListItemNodeName), serializer);
            }
        }



        public void Deserialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (key == null) throw new ArgumentNullException("key");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", "key");

            if (!typeof(List<TListItemType>).IsAssignableFrom(type))
                throw new WrongSerializerException(target != null ? target.GetType() : type, typeof(List<TListItemType>));

            var list = target as List<TListItemType>;
            var itemSerializer = serializer.SerializerSelector.GetSerializer(typeof(TListItemType));

            if (!itemSerializer.Any())
                throw new NoSerializerFoundException(typeof(TListItemType));

            if (!config.HasNode(key))
            {
                list.Do(l => l.Clear());
                return;
            }

            list = list ?? new List<TListItemType>();

            foreach (var itemNode in config.GetNode(key).GetNodes(ListItemNodeName))
            {
                var item = CreateDefaultListItem();
                var objItem = (object) item;

                itemSerializer.Single()
                    .Deserialize(typeof (TListItemType), ref objItem, typeof (TListItemType).FullName, itemNode, serializer);

                item = (TListItemType) objItem;

                list.Add(item);
            }

            target = list;
        }


        private static TListItemType CreateDefaultListItem()
        {
            var tlt = typeof(TListItemType);

            if (tlt.IsValueType)
                return default(TListItemType);

            if (typeof(string) == tlt)
                return (TListItemType)(object)string.Empty; // no default constructor for string

            if (!tlt.IsAbstract && tlt.GetConstructors().Any(c => c.GetParameters().Length == 0))
                return Activator.CreateInstance<TListItemType>();

            throw new ArgumentException("No suitable default constructor for " + tlt.Name);
        }

        //public void Serialize(
        //    Type type,
        //    object target,
        //    string uniqueKey,
        //    ConfigNode config,
        //    IConfigNodeSerializer serializer)
        //{
        //    if (type == null) throw new ArgumentNullException("type");
        //    if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
        //    if (config == null) throw new ArgumentNullException("config");
        //    if (serializer == null) throw new ArgumentNullException("serializer");
        //    if (string.IsNullOrEmpty(uniqueKey))
        //        throw new ArgumentException("Invalid key: " + uniqueKey, "uniqueKey");

        //    if (!typeof(List<TListItemType>).IsAssignableFrom(type))
        //        throw new WrongSerializerException(type, typeof(List<TListItemType>));

        //    if (config.HasNode(uniqueKey))
        //        throw new ConfigNodeDuplicateKeyException(uniqueKey);

        //    var itemSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(typeof(TListItemType));
        //    if (!itemSerializer.Any() && type.IsValueType)
        //        throw new NoSerializerFoundException(type);

        //    var node = config.AddNode(uniqueKey);
        //    var list = target as List<TListItemType>;

        //    if (list == null)
        //        return;

        //    foreach (var item in list)
        //    {
        //        var itemConfig = new ConfigNode(typeof(TListItemType).Name);
        //        TListItemType item1 = item;

        //        // use surrogate if available
        //        //itemSerializer.Do(
        //        //    s => s.Serialize(typeof (TListItemType), item1, typeof (TListItemType).Name, itemConfig, serializer));

        //        //// otherwise a general serialize will work if it's a reference type
        //        //itemSerializer.SingleOrDefault().IfNull(() => serializer.Serialize(item1, itemConfig));
        //        serializer.Serialize(item1, itemConfig);
        //        node.AddNode(itemConfig);

        //        //var itemConfig = serializer.CreateConfigNodeFromObject(item);

        //        //itemConfig.Do(cfg =>
        //        //{
        //        //    itemConfig.name = ListItemNodeName;
        //        //    node.AddNode(itemConfig);
        //        //});
        //    }
        //}


        //public object Deserialize(
        //    Type type,
        //    object target,
        //    string uniqueKey,
        //    ConfigNode config,
        //    IConfigNodeSerializer serializer)
        //{
        //    if (type == null) throw new ArgumentNullException("type");
        //    if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
        //    if (config == null) throw new ArgumentNullException("config");
        //    if (serializer == null) throw new ArgumentNullException("serializer");
        //    if (string.IsNullOrEmpty(uniqueKey))
        //        throw new ArgumentException("Invalid key: " + uniqueKey, "uniqueKey");

        //    if (!typeof(List<TListItemType>).IsAssignableFrom(type))
        //        throw new WrongSerializerException(type, typeof(List<TListItemType>));

        //    var itemSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(typeof(TListItemType));
        //    if (!itemSerializer.Any() && type.IsValueType)
        //        throw new NoSerializerFoundException(type);

        //    var list = target as List<TListItemType>;


        //    list = list ?? Activator.CreateInstance(type) as List<TListItemType>;
        //    if (list == null) return null; // we failed to create list somehow...

        //    list.Do(l =>
        //    {
        //        l.Clear();

        //        config
        //            .With(cfg => cfg.GetNode(uniqueKey))
        //            .With(c => c.GetNodes(typeof(TListItemType).Name))
        //            .ToList()
        //            .ForEach(itemNode =>
        //            {
        //                var item = CreateDefaultListItem();

        //                serializer.Deserialize(item, itemNode);
        //                l.Add(item);

        //                //                        if (itemSerializer.Any())
        //                //                        {
        //                //                            item = (TListItemType)
        //                //                                itemSerializer.Return(
        //                //                                    s =>
        //                //                                        s.Deserialize(typeof (TListItemType), default(TListItemType),
        //                //                                            typeof (TListItemType).Name,
        //                //                                            itemNode, serializer), default(TListItemType));
        //                //                            l.Add(item);
        //                //                        }
        //                //                        else
        //                //                        {
        //                //// ReSharper disable once CompareNonConstrainedGenericWithNull
        //                //                            if (typeof (TListItemType).IsValueType || (item != null))
        //                //                                serializer.Deserialize(item, itemNode);
        //                //                            l.Add(item);
        //                //                        }
        //            });
        //    });

        //    return list;
        //}


        //private static TListItemType CreateDefaultListItem()
        //{
        //    var tlt = typeof(TListItemType);

        //    if (tlt.IsValueType)
        //        return default(TListItemType);

        //    if (typeof(string) == tlt)
        //        return (TListItemType)(object)string.Empty; // no default constructor for string

        //    if (!tlt.IsAbstract && tlt.GetConstructors().Any(c => c.GetParameters().Length == 0))
        //        return Activator.CreateInstance<TListItemType>();

        //    throw new ArgumentException("No suitable default constructor for " + tlt.Name);
        //}

    }
}
