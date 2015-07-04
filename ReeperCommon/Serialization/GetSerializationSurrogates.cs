using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace ReeperCommon.Serialization
{
    /// <summary>
    /// Retrieve surrogates from target assembly. Note that abstract and generics aren't permitted
    /// </summary>
    public class GetSerializationSurrogates : IGetSerializationSurrogates
    {
        private readonly IGetSurrogateSupportedTypes _surrogateSupportedTypesQuery;

        public GetSerializationSurrogates(IGetSurrogateSupportedTypes surrogateSupportedTypesQuery)
        {
            if (surrogateSupportedTypesQuery == null) throw new ArgumentNullException("surrogateSupportedTypesQuery");
            _surrogateSupportedTypesQuery = surrogateSupportedTypesQuery;
        }


        public IEnumerable<Type> Get(Assembly fromAssembly)
        {
            return fromAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsVisible && !t.IsAbstract && !t.ContainsGenericParameters)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null && t.GetConstructor(Type.EmptyTypes).IsPublic)
                .Where(ImplementsGenericSerializationSurrogateInterface);
        }


        private bool ImplementsGenericSerializationSurrogateInterface(Type typeCheck)
        {
            return _surrogateSupportedTypesQuery.Get(typeCheck).Any();
        }
    }
}
