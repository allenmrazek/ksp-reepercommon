using System;
using System.Collections.Generic;
using System.Linq;

namespace ReeperCommon.Serialization
{
    public class GetSurrogateSupportedTypes : IGetSurrogateSupportedTypes
    {
        // the serialization surrogateSerializer could have multiple supported types
        // Consider:
        //   MySurrogate : IConfigNodeItemSerializer<bool>, IConfigNodeItemSerializer<string> etc
        public IEnumerable<Type> Get(Type surrogateType)
        {
            return surrogateType.GetInterfaces()
              .Where(interfaceType => interfaceType.IsGenericType &&
                                      typeof(IConfigNodeItemSerializer).IsAssignableFrom(interfaceType))
              .Select(interfaceType => interfaceType.GetGenericArguments().First());
        }
    }
}
