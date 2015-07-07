using System;
using ReeperCommon.Containers;

namespace ReeperCommon.Serialization
{
    public interface IConfigNodeItemSerializerSelector
    {
        Maybe<IConfigNodeItemSerializer> GetSerializer(Type target);
    }
}
