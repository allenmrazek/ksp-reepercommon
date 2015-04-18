using System.Collections.Generic;
using System.Reflection;

namespace ReeperCommon.Serialization
{
    public interface IFieldInfoQuery
    {
        IEnumerable<FieldInfo> Get(object target);
    }
}
