using System;
using System.Collections.Generic;
using System.Linq;

namespace ReeperCommon.Serialization
{
    public class GetSurrogateSupportedTypes : IGetSurrogateSupportedTypes
    {
        // the serialization surrogateSerializer could have multiple supported types
        // Consider:
        //   MySurrogate : ISurrogateSerializer<bool>, ISurrogateSerializer<string> etc
        public IEnumerable<Type> Get(Type surrogateType)
        {
            //var interfaces = surrogateType.GetInterfaces();

            //var genericInterfaces = interfaces.Where(interfaceType => interfaceType.IsGenericType);

            //var whichImplementSurrogate = genericInterfaces.Where(typeof (ISurrogateSerializer).IsAssignableFrom);

            //return whichImplementSurrogate.Select(it => it.GetGenericArguments().First());

            return surrogateType.GetInterfaces()
              .Where(interfaceType => interfaceType.IsGenericType &&
                                      typeof(ISurrogateSerializer).IsAssignableFrom(interfaceType))
              .Select(interfaceType => interfaceType.GetGenericArguments().First());
        }
    }
}
