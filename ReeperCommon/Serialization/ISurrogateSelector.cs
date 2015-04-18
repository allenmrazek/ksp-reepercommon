using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public interface ISurrogateSelector
    {
        Maybe<ISerializationSurrogate> GetSurrogate(Type target);
    }
}
