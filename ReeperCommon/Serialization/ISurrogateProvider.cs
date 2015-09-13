using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public interface ISurrogateProvider
    {
        Maybe<IConfigNodeItemSerializer> Get(Type toBeSerialized);
    }
}
