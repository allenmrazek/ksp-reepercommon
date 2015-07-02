using System.Collections.Generic;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    public interface IGetFieldInfo
    {
        IEnumerable<FieldInfo> Get(object target);
    }
}
