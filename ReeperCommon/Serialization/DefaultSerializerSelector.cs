using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultSerializerSelector : ISerializerSelector
    {
        private readonly Dictionary<Type, ISerializationSurrogate> _surrogates = new Dictionary<Type, ISerializationSurrogate>();

        public DefaultSerializerSelector(ISurrogateProvider surrogateProvider = null)
        {
            if (surrogateProvider == null) return;

            foreach (var item in surrogateProvider.Get())
                if (_surrogates.ContainsKey(item.Key))
                    throw new ArgumentException("Surrogate provider has provided duplicate surrogates for type " +
                                                item.Key.FullName);
                else 
                    _surrogates.Add(item.Key, item.Value);
        }


        public void AddSurrogate(Type target, ISerializationSurrogate surrogate)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (surrogate == null) throw new ArgumentNullException("surrogate");

            if (_surrogates.ContainsKey(target))
                throw new InvalidOperationException("Already have surrogate for type " + target.FullName);

            _surrogates.Add(target, surrogate);
        }


        public void AddSurrogate<T>(ISerializationSurrogate<T> surrogate)
        {
            if (surrogate == null) throw new ArgumentNullException("surrogate");

            AddSurrogate(typeof (T), surrogate);
        }


        private static Maybe<ISerializationNative> GetNative(Type target)
        {
            return typeof (IReeperPersistent).IsAssignableFrom(target)
                ? Maybe<ISerializationNative>.With(new NativeSerializer(target))
                : Maybe<ISerializationNative>.None;
        }


        private Maybe<ISerializationSurrogate> GetSurrogate(Type target)
        {
            if (target == null) throw new ArgumentNullException("target");

            ISerializationSurrogate surrogate;

            return _surrogates.TryGetValue(target, out surrogate) ?
                Maybe<ISerializationSurrogate>.With(surrogate)
                    :
                Maybe<ISerializationSurrogate>.None;
        }


        public virtual Maybe<ISerializer> GetSerializer(Type target)
        {
            var native = GetNative(target);
            var surrogate = GetSurrogate(target);

            if (native.Any() || surrogate.Any()) return Maybe<ISerializer>.With(native.Any()
                ? (ISerializer)native.Single() : surrogate.Single());

            return Maybe<ISerializer>.None;
        }
    }
}
