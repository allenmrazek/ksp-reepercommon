using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using FactoryMethodList = System.Collections.Generic.List<ReeperCommon.Serialization.SurrogateFactoryMethod>;


namespace ReeperCommon.Serialization
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class SurrogateProvider : ISurrogateProvider
    {
        // This simply prevents creating a new serializer on every single request, by lazily
        // creating them as requested and then keeping them for reuse
        protected class Surrogate
        {
            private readonly Lazy<Maybe<IConfigNodeItemSerializer>> _surrogate;
 
            public Surrogate(IEnumerable<SurrogateFactoryMethod> factories) // note: first successful factory result will be used
            {
                if (factories == null) throw new ArgumentNullException("factories");

                _surrogate = new Lazy<Maybe<IConfigNodeItemSerializer>>(() =>
                {
                    foreach (var f in factories)
                    {
                        var surrogate = f();
                        if (surrogate.Any())
                            return surrogate;
                    }

                    return Maybe<IConfigNodeItemSerializer>.None;
                });
            }

            public Maybe<IConfigNodeItemSerializer> Value
            {
                get { return _surrogate.Value; }
            } 
        }

        private readonly IGetSerializationSurrogates _getSerializationSurrogates;
        private readonly IGetSurrogateSupportedTypes _getSurrogateSupportedTypes;
        private readonly IEnumerable<Assembly> _assembliesToSearch;

// ReSharper disable once FieldCanBeMadeReadOnly.Global
// ReSharper disable once MemberCanBePrivate.Global
        protected Lazy<Dictionary<Type, Surrogate>> Surrogates;


        public SurrogateProvider(
            IGetSerializationSurrogates getSerializationSurrogates, 
            IGetSurrogateSupportedTypes getSurrogateSupportedTypes,
            IEnumerable<Assembly> assembliesToSearch)
        {
            if (getSerializationSurrogates == null) throw new ArgumentNullException("getSerializationSurrogates");
            if (getSurrogateSupportedTypes == null) throw new ArgumentNullException("getSurrogateSupportedTypes");
            if (assembliesToSearch == null) throw new ArgumentNullException("assembliesToSearch");
       
            _getSerializationSurrogates = getSerializationSurrogates;
            _getSurrogateSupportedTypes = getSurrogateSupportedTypes;
            _assembliesToSearch = assembliesToSearch;

            Surrogates = new Lazy<Dictionary<Type, Surrogate>>(Initialize);
        }


        protected virtual Dictionary<Type, Surrogate> Initialize()
        {
            var surrogateTypes = _assembliesToSearch
                .SelectMany(targetAssembly => _getSerializationSurrogates.Get(targetAssembly))
                .Where(t => !t.IsAbstract && !t.ContainsGenericParameters)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null && t.GetConstructor(Type.EmptyTypes).IsPublic)
                .Where(t => _getSurrogateSupportedTypes.Get(t).Any());

            return surrogateTypes.ToDictionary(surrogateType => surrogateType,
                surrogateType =>
                    new Surrogate(
                        new FactoryMethodList(
                            _getSurrogateSupportedTypes.Get(surrogateType)
                                .Select(surrogateSupportedType =>
                                {
                                    SurrogateFactoryMethod m = () => CreateSurrogate(surrogateSupportedType, surrogateType);

                                    return m;
                                })
                        )
                    )
                );
        }


        private static Maybe<IConfigNodeItemSerializer> CreateSurrogate(Type typeToBeSerialized, Type surrogateType)
        {
            if (typeToBeSerialized == null) throw new ArgumentNullException("typeToBeSerialized");
            if (surrogateType == null) throw new ArgumentNullException("surrogateType");

            if (typeof(IConfigNodeItemSerializer).IsAssignableFrom(surrogateType))
                throw new ArgumentException(surrogateType.FullName + " cannot be assigned to IConfigNodeItemSerializer");


            if (!surrogateType.IsGenericTypeDefinition)
                return (Activator.CreateInstance(surrogateType) as IConfigNodeItemSerializer).ToMaybe();

            if (surrogateType.GetGenericArguments().Length != 1)
                return Maybe<IConfigNodeItemSerializer>.None;

            surrogateType = surrogateType.MakeGenericType(new[] {typeToBeSerialized});

            return (Activator.CreateInstance(surrogateType) as IConfigNodeItemSerializer).ToMaybe();
        }


        public Maybe<IConfigNodeItemSerializer> Get(Type toBeSerialized)
        {
            if (toBeSerialized == null) throw new ArgumentNullException("toBeSerialized");

            Surrogate surrogate;

            return !Surrogates.Value.TryGetValue(toBeSerialized, out surrogate) ? Maybe<IConfigNodeItemSerializer>.None : surrogate.Value;
        }
    }
}
