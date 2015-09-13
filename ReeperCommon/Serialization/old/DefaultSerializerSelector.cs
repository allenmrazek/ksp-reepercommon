//using System;
//using System.Linq;
//using ReeperCommon.Containers;

//namespace ReeperCommon.Serialization
//{
//    public class DefaultSerializerSelector : SerializerSelector
//    {
//        //public DefaultSerializerSelector()
//        //    : this(new DefaultSurrogateProvider(new GetSerializationSurrogates(new GetSurrogateSupportedTypes()), new GetSurrogateSupportedTypes()))
//        //{

//        //}

//        public DefaultSerializerSelector(ISurrogateProvider surrogates)
//        {
//            surrogates.Get().ToList().ForEach(kvp => AddSerializer(kvp.Key, kvp.Value));
//        }


//        public override Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
//        {
//            var serializer = base.GetSerializer(target);

//            if (serializer.Any())
//            {
//                // decorate here
//            }

//            return serializer;
//        }


//        protected virtual Maybe<IConfigNodeItemSerializer> GetNativeSerializer(Type target)
//        {

//        }
//    }
//}

////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Reflection;
////using ReeperCommon.Containers;

////namespace ReeperCommon.Serialization
////{
////    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
////    public class DefaultConfigNodeItemSerializerSelector : IConfigNodeItemSerializerSelector
////    {
////        public delegate Maybe<IConfigNodeItemSerializer> SurrogateFactoryMethod(Type target);

////        private class SurrogateInfo
////        {
////            public readonly Type SurrogateType;
////            public readonly SurrogateFactoryMethod Factory;

////            public SurrogateInfo(Type surrogateType, SurrogateFactoryMethod factory)
////            {
////                if (surrogateType == null) throw new ArgumentNullException("surrogateType");
////                if (factory == null) throw new ArgumentNullException("factory");

////                SurrogateType = surrogateType;
////                Factory = factory;
////            }


////            public SurrogateInfo(Type surrogateType)
////                : this(
////                    surrogateType,
////                    t =>
////                        Maybe<IConfigNodeItemSerializer>.With(
////                            (IConfigNodeItemSerializer)Activator.CreateInstance(surrogateType)))
////            {
////                if (surrogateType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
////                        null, Type.EmptyTypes, null) == null)
////                    throw new ArgumentException(surrogateType.FullName +
////                                                " must implement a default constructor or else supply a custom factory method");
////            }


////            public SurrogateInfo(Type surrogateType, IConfigNodeItemSerializer serializer) :
////                this(surrogateType, t => Maybe<IConfigNodeItemSerializer>.With(serializer))
////            {
////                if (surrogateType == null) throw new ArgumentNullException("surrogateType");
////                if (serializer == null) throw new ArgumentNullException("serializer");
////            }
////        }


////        // ReSharper disable once MemberCanBePrivate.Global
////        public readonly ISurrogateProvider SurrogateProvider;
////        private readonly Dictionary<Type, List<SurrogateInfo>> _surrogates =
////            new Dictionary<Type, List<SurrogateInfo>>();



////        public DefaultConfigNodeItemSerializerSelector(ISurrogateProvider surrogateProvider = null)
////        {
////            if (surrogateProvider == null) return;

////            SurrogateProvider = surrogateProvider;

////            SurrogateProvider.Get().ToList().ForEach(kvp => AddSerializer(kvp.Key, kvp.Value));

////            // support for List<T>
////            //AddSerializer(typeof(List<>), CreateGenericInstance);
////        }


////        private void AddSerializer(Type target, SurrogateInfo info)
////        {
////            if (target == null) throw new ArgumentNullException("target");
////            if (info == null) throw new ArgumentNullException("info");

////            List<SurrogateInfo> surrogatesForType;

////            if (_surrogates.TryGetValue(target, out surrogatesForType))
////                surrogatesForType.Add(info);
////            else
////                _surrogates.Add(target, new List<SurrogateInfo> { info });
////        }


////        public void AddSerializer(Type target, IConfigNodeItemSerializer surrogateSerializer)
////        {
////            if (target == null) throw new ArgumentNullException("target");
////            if (surrogateSerializer == null) throw new ArgumentNullException("surrogateSerializer");

////            AddSerializer(target, new SurrogateInfo(target, surrogateSerializer));
////        }


////        public void AddSerializer<T>(IConfigNodeItemSerializer surrogateSerializer)
////        {
////            if (surrogateSerializer == null) throw new ArgumentNullException("surrogateSerializer");

////            AddSerializer(typeof(T), surrogateSerializer);
////        }


////        public void AddSerializer(Type target, SurrogateFactoryMethod factory)
////        {
////            if (target == null) throw new ArgumentNullException("target");
////            if (factory == null) throw new ArgumentNullException("factory");

////            AddSerializer(target, new SurrogateInfo(target, factory));
////        }


////        protected virtual Maybe<IConfigNodeItemSerializer> GetNative(Type target)
////        {
////            return typeof(IReeperPersistent).IsAssignableFrom(target)
////                ? Maybe<IConfigNodeItemSerializer>.With(new NativeSerializer())
////                : Maybe<IConfigNodeItemSerializer>.None;
////        }


////        protected virtual Maybe<IConfigNodeItemSerializer> GetSurrogate(Type target)
////        {
////            if (target == null) throw new ArgumentNullException("target");

////            List<SurrogateInfo> potentialSurrogatesForType;

////            if (!_surrogates.TryGetValue(target, out potentialSurrogatesForType))
////                return target.IsGenericType ? GetGenericSurrogate(target) : Maybe<IConfigNodeItemSerializer>.None;

////            foreach (var createdInstanceOfSurrogate in potentialSurrogatesForType
////                .Select(potential => potential.Factory(target))
////                .Where(createdInstanceOfSurrogate => createdInstanceOfSurrogate.Any()))
////                return createdInstanceOfSurrogate;

////            return target.IsGenericType ? GetGenericSurrogate(target) : Maybe<IConfigNodeItemSerializer>.None;
////        }


////        /// <summary>
////        /// Attempts to locate a serializer that has been specifically added to support
////        /// multiple target types with one surrogate instance.
////        /// 
////        /// For example, if I had a Setting<T>, I might AddSurrogate(typeof(Setting<>), someInstance)
////        /// to have someInstance handle serialization of all Setting variants. This puts a lot of
////        /// complexity into the surrogate itself, however, so use it sparingly
////        /// </summary>
////        /// <param name="target"></param>
////        /// <returns></returns>
////        protected virtual Maybe<IConfigNodeItemSerializer> GetGenericSurrogate(Type target)
////        {
////            if (target == null) throw new ArgumentNullException("target");

////            List<SurrogateInfo> genericSurrogatesForTarget;

////            var genericDefinition = target.GetGenericTypeDefinition();
////            if (genericDefinition == null) throw new Exception("Couldn't get generic definition of " + target.FullName);

////            if (!_surrogates.TryGetValue(genericDefinition, out genericSurrogatesForTarget))
////                return Maybe<IConfigNodeItemSerializer>.None;

////            foreach (var createdInstanceOfSurrogate in genericSurrogatesForTarget
////                .Select(potential => potential.Factory(target))
////                .Where(createdInstanceOfSurrogate => createdInstanceOfSurrogate.Any()))
////                return createdInstanceOfSurrogate;

////            return Maybe<IConfigNodeItemSerializer>.None;
////        }


////        public virtual Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
////        {
////            var native = GetNative(target);
////            var surrogate = GetSurrogate(target);

////            if (!native.Any() && !surrogate.Any())
////                return Maybe<IConfigNodeItemSerializer>.None;

////            return Maybe<IConfigNodeItemSerializer>.With(new ReeperPersistentMethodCaller(
////                native.SingleOrDefault() ?? surrogate.SingleOrDefault()));
////        }


////        public override string ToString()
////        {
////            return "DefaultConfigNodeItemSerializerSelector: surrogates: " +
////                   string.Join(",", _surrogates.Keys.Select(k => k.FullName).ToArray());
////        }


////        //private static Maybe<IConfigNodeItemSerializer> CreateGenericInstance(Type type)
////        //{
////        //    if (type == null) throw new ArgumentNullException("type");

////        //    return type.GetGenericArguments().Any() && type.GetGenericArguments().Length == 1
////        //        ? ((IConfigNodeItemSerializer)
////        //            Activator.CreateInstance(
////        //                typeof(ListSurrogate<>).MakeGenericType(type.GetGenericArguments()[0]))).ToMaybe()
////        //        : Maybe<IConfigNodeItemSerializer>.None;

////        //}
////    }
////}
