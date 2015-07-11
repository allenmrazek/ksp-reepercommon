using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DefaultSurrogateProvider : ISurrogateProvider
    {
        private readonly IGetSerializationSurrogates _getSerializationSurrogates;
        private readonly IGetSurrogateSupportedTypes _getSurrogateSupportedTypes;


        public DefaultSurrogateProvider() : this(new GetSerializationSurrogates(
            new GetSurrogateSupportedTypes()), 
            new GetSurrogateSupportedTypes())
        {
        }


        public DefaultSurrogateProvider(IGetSerializationSurrogates getSerializationSurrogates, IGetSurrogateSupportedTypes getSurrogateSupportedTypes)
        {
            if (getSerializationSurrogates == null) throw new ArgumentNullException("getSerializationSurrogates");
            if (getSurrogateSupportedTypes == null) throw new ArgumentNullException("getSurrogateSupportedTypes");

            _getSerializationSurrogates = getSerializationSurrogates;
            _getSurrogateSupportedTypes = getSurrogateSupportedTypes;
        }


        public virtual IEnumerable<KeyValuePair<Type, IConfigNodeItemSerializer>> Get()
        {
            return GetTargets()
                .SelectMany(targetAssembly => _getSerializationSurrogates.Get(targetAssembly))
                .Where(t => !t.IsAbstract && !t.ContainsGenericParameters)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null && t.GetConstructor(Type.EmptyTypes).IsPublic)
                .Where(t => _getSurrogateSupportedTypes.Get(t).Any())
                .SelectMany(
                    t =>
                        _getSurrogateSupportedTypes.Get(t)
                            .Select(
                                surrogateIdentifier =>
                                    new KeyValuePair<Type, IConfigNodeItemSerializer>(surrogateIdentifier,
                                        Activator.CreateInstance(t) as IConfigNodeItemSerializer)));
        }


        public virtual IEnumerable<Assembly> GetTargets()
        {
            return new [] {Assembly.GetExecutingAssembly()};
        }
    }
}
