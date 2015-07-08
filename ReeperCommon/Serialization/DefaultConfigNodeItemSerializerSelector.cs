using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Serialization.Exceptions;

namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultConfigNodeItemSerializerSelector : IConfigNodeItemSerializerSelector
    {
        private readonly Dictionary<Type, ISurrogateSerializer> _surrogates = new Dictionary<Type, ISurrogateSerializer>();
        public readonly ISurrogateProvider SurrogateProvider;

        public DefaultConfigNodeItemSerializerSelector(ISurrogateProvider surrogateProvider = null)
        {
            if (surrogateProvider == null) return;

            SurrogateProvider = surrogateProvider;

            SurrogateProvider.Get().ToList().ForEach(kvp => AddSurrogate(kvp.Key, kvp.Value));
        }


        public void AddSurrogate(Type target, ISurrogateSerializer surrogateSerializer)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (surrogateSerializer == null) throw new ArgumentNullException("surrogateSerializer");

            if (_surrogates.ContainsKey(target))
                throw new DuplicateSurrogateException(target);

            _surrogates.Add(target, surrogateSerializer);
        }


        public void AddSurrogate<T>(ISurrogateSerializer<T> surrogateSerializer)
        {
            if (surrogateSerializer == null) throw new ArgumentNullException("surrogateSerializer");

            AddSurrogate(typeof (T), surrogateSerializer);
        }


        protected static Maybe<INativeSerializer> GetNative(Type target)
        {
            return typeof (IReeperPersistent).IsAssignableFrom(target)
                ? Maybe<INativeSerializer>.With(new NativeSerializer())
                : Maybe<INativeSerializer>.None;
        }


        protected Maybe<ISurrogateSerializer> GetSurrogate(Type target)
        {
            if (target == null) throw new ArgumentNullException("target");

            ISurrogateSerializer surrogateSerializer;

            return _surrogates.TryGetValue(target, out surrogateSerializer) ?
                Maybe<ISurrogateSerializer>.With(surrogateSerializer)
                    :
                    target.IsGenericType ? GetGenericSurrogate(target) : Maybe<ISurrogateSerializer>.None;
        }


        private Maybe<ISurrogateSerializer> GetGenericSurrogate(Type target)
        {
            if (target == null) throw new ArgumentNullException("target");

            ISurrogateSerializer surrogateSerializer;

            var genericDefinition = target.GetGenericTypeDefinition();
            if (genericDefinition == null) throw new Exception("Couldn't get generic definition of " + target.FullName);

            return _surrogates.TryGetValue(genericDefinition, out surrogateSerializer)
                ? Maybe<ISurrogateSerializer>.With(surrogateSerializer)
                : Maybe<ISurrogateSerializer>.None;
        }


        public virtual Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
        {
            var native = GetNative(target);
            var surrogate = GetSurrogate(target);

            if (!native.Any() && !surrogate.Any())
                return Maybe<IConfigNodeItemSerializer>.None;

            return Maybe<IConfigNodeItemSerializer>.With(new ReeperPersistentMethodCaller(
                (IConfigNodeItemSerializer)native.SingleOrDefault() ?? surrogate.SingleOrDefault()));
        }
    }
}
