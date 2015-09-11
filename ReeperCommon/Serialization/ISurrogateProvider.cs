using System;
using System.Collections.Generic;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public delegate Maybe<IConfigNodeItemSerializer> SurrogateFactory(Type target);

    public interface ISurrogateProvider
    {
        IEnumerable<KeyValuePair<Type, IConfigNodeItemSerializer>> Get();
    }
}
