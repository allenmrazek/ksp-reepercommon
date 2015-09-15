using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public class GenericSurrogateProvider : ISurrogateProvider
    {
        public GenericSurrogateProvider(IEnumerable<Assembly> assembliesToScan)
        {
            
        }


        public Maybe<IConfigNodeItemSerializer> Get(Type toBeSerialized)
        {
            throw new NotImplementedException();
        }
    }
}
