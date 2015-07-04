using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public interface ISerializerSelector
    {
        Maybe<ISerializer> GetSerializer(Type target);
    }
}
