using System.Collections.Generic;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    public interface IGetObjectFields
    {
        IEnumerable<FieldInfo> Get(object target);
    }
}
