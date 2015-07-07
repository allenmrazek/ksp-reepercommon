using System;
using System.Collections.Generic;

namespace ReeperCommon.Serialization
{
    public interface ISurrogateProvider
    {
        IEnumerable<KeyValuePair<Type, ISurrogateSerializer>> Get();
    }
}
