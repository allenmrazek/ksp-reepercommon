using System;
using System.Collections;
using System.Collections.Generic;

namespace ReeperCommon.Serialization
{
    public interface IGetSurrogateSupportedTypes
    {
        IEnumerable<Type> Get(Type surrogateType);
    }
}
