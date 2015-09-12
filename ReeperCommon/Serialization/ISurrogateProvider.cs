using System;
using System.Collections.Generic;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{


    public interface ISurrogateProvider
    {
        IEnumerable<KeyValuePair<Type, IConfigNodeItemSerializer>> Get();
    }
}
