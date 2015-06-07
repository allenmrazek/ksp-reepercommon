using System;
using System.Collections.Generic;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultSurrogateSelector : ISurrogateSelector
    {
        private readonly Dictionary<Type, ISerializationSurrogate> _surrogates = new Dictionary<Type, ISerializationSurrogate>();

        public DefaultSurrogateSelector(ISurrogateProvider surrogateProvider = null)
        {
            if (surrogateProvider == null) return;

            foreach (var item in surrogateProvider.Get())
                if (_surrogates.ContainsKey(item.Key))
                    throw new ArgumentException("Surrogate provider has provided duplicate surrogates for type " +
                                                item.Key.FullName);
                else 
                    _surrogates.Add(item.Key, item.Value);
        }


        public virtual
            Maybe<ISerializationSurrogate> GetSurrogate(Type target)
        {
            if (target == null) throw new ArgumentNullException("target");

            ISerializationSurrogate surrogate;

            return _surrogates.TryGetValue(target, out surrogate) ?
                Maybe<ISerializationSurrogate>.With(surrogate)
                    :
                Maybe<ISerializationSurrogate>.None;
        }


        public virtual void AddSurrogate(Type target, ISerializationSurrogate surrogate)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (surrogate == null) throw new ArgumentNullException("surrogate");

            if (_surrogates.ContainsKey(target))
                throw new InvalidOperationException("Already have surrogate for type " + target.FullName);

            _surrogates.Add(target, surrogate);
        }


        public virtual void AddSurrogate<T>(ISerializationSurrogate<T> surrogate)
        {
            if (surrogate == null) throw new ArgumentNullException("surrogate");

            AddSurrogate(typeof (T), surrogate);
        }
    }
}
